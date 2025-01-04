using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Options;
using CourseAI.Application.Services;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities;
using CourseAI.Domain.Entities.Identity;
using FluentEmail.Core;
using Mediator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OneOf;

namespace CourseAI.Application.Features.Users.MagicLink;

public class SendMagicLinkHandler(
    ILogger<IHandler<SendMagicLinkRequest, string>> logger,
    IFluentEmail fluentEmail,
    AppDbContext dbContext,
    IEmailVerificationLinkFactory emailVerificationLinkFactory,
    IOptions<EmailOptions> emailOptions)
    : IHandler<SendMagicLinkRequest, string>
{
    public async ValueTask<OneOf<string, Error>> Handle(SendMagicLinkRequest request, CancellationToken ct)
    {
        var user = await dbContext.Users.FindAsync([request.UserId], ct);

        if (user == null)
        {
            return Error.NotFound($"User not found {request.UserId}");
        }

        var utcNow = DateTime.UtcNow;
        var verificationToken = new EmailVerificationToken
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            CreatedOnUtc = utcNow,
            ExpiresOnUtc = utcNow.AddHours(24)
        };

        dbContext.EmailVerificationTokens.Add(verificationToken);
        await dbContext.SaveChangesAsync(ct);

        var verificationLink = emailVerificationLinkFactory.Create(verificationToken);

        logger.LogWarning($"Sending email verification link to {user.Email}, verificationLink: {verificationLink}");

        if (verificationLink == null)
        {
            return Error.ServerError($"Failed to generate verification link for user {request.UserId}");
        }

        var emailOptionsValue = emailOptions.Value;

        logger.LogWarning(
            $"All email options: port {emailOptionsValue.Port}, sender {emailOptionsValue.Sender}, sender email {emailOptionsValue.SenderEmail}, host {emailOptionsValue.Host}, enable ssl {emailOptionsValue.EnableSsl}, username {emailOptionsValue.Username}, password {emailOptionsValue.Password}");

        var email = await fluentEmail
            .To(user.Email)
            .Subject("Email Verification to Levenue Courses")
            .Body(GetEmailBody(verificationLink), isHtml: true)
            // .Body($"Please verify your email by clicking <a href={verificationLink}>this link</a>", isHtml: true)
            .SendAsync();

        if (!email.Successful)
            foreach (var error in email.ErrorMessages)
            {
                logger.LogError($"Error sending email: {error}");
            }

        return emailOptions.Value.Sender;
    }

    private string GetEmailBody(string verificationLink)
    {
        return $@"
    <!DOCTYPE html>
    <html>
    <head>
        <meta charset='UTF-8'>
        <title>Email Verification</title>
        <style>
            /* Mobile Styles */
            @media only screen and (max-width: 600px) {{
                .container {{
                    width: 100% !important;
                }}
                .button {{
                    padding: 15px 20px !important;
                    font-size: 16px !important;
                }}
                .header, .content, .footer {{
                    padding: 20px 10px !important;
                }}
                .bottom-footer {{
                    padding: 15px 0 !important;
                }}
            }}
        </style>
    </head>
    <body style='font-family: Arial, sans-serif; background-color: #f9f9f9; margin: 0; padding: 0;'>
        <table width='100%' cellpadding='0' cellspacing='0' border='0' style='background-color: #f9f9f9; padding: 20px 0;'>
            <tr>
                <td align='center'>
                    <table class='container' width='600' cellpadding='0' cellspacing='0' border='0' style='background-color: #ffffff; border-radius: 8px; box-shadow: 0 4px 6px rgba(0,0,0,0.1); overflow: hidden;'>
                        <!-- Header Section -->
                        <tr>
                            <td class='header' align='center' style='padding: 40px 20px 20px 20px;'>
                                <h2 style='color: #333333; margin: 0;'>Sign in to Levenue Courses</h2>
                            </td>
                        </tr>
                        <!-- Content Section -->
                        <tr>
                            <td class='content' align='center' style='padding: 20px;'>
                                <p style='color: #555555; font-size: 16px; margin: 0 0 20px 0;'>Please verify your email by clicking the button below:</p>
                                <a href='{verificationLink}' class='button' style='background-color: hsl(20, 48%, 72%); color: #ffffff; padding: 15px 30px; text-decoration: none; font-size: 18px; border-radius: 5px; display: inline-block;'>Sign in</a>
                            </td>
                        </tr>
                        <!-- Footer Section -->
                        <tr>
                            <td class='footer' align='center' style='padding: 20px; background-color: #f2f2f2; border-bottom-left-radius: 8px; border-bottom-right-radius: 8px;'>
                                <p style='color: #777777; font-size: 12px; margin: 0;'>If you did not request this email, you can safely ignore it.</p>
                            </td>
                        </tr>
                    </table>
                    <!-- Bottom Footer -->
                    <table width='600' cellpadding='0' cellspacing='0' border='0' style='max-width: 600px; width: 100%; margin-top: 10px;'>
                        <tr>
                            <td class='bottom-footer' align='center' style='padding: 20px 0;'>
                                <p style='color: #aaaaaa; font-size: 12px; margin: 0;'>© {DateTime.Now.Year} Levenue Courses. All rights reserved.</p>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </body>
    </html>
    ";
    }
}