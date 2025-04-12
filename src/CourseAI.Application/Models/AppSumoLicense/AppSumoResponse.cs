namespace CourseAI.Application.Models.AppSumoLicense;

public class AppSumoResponse
{
    public string Status { get; set; }
    public string PlanId { get; set; }
    public bool IsRefunded { get; set; }
    public string InvoiceItemUuid { get; set; }
}

public class AppSumoLicenseModel
{
    public int Id { get; set; }
    public string LicenseKey { get; set; }
    public string Plan { get; set; }  // "creator", "studio"
    public bool IsActive { get; set; }
    public DateTime ActivatedAt { get; set; }
    public string UserId { get; set; }  // FK to your User table
}

// Models/AppSumo/TokenResponse.cs
public class AppSumoTokenResponse
{
    public string access_token { get; set; }
    public string token_type { get; set; }
    public int expires_in { get; set; }
    public string refresh_token { get; set; }
    public string id_token { get; set; }
    public string error { get; set; }
}

// Models/AppSumo/LicenseResponse.cs
public class AppSumoLicenseResponse
{
    public string license_key { get; set; }
    public string status { get; set; }
    public List<string> scopes { get; set; }
}

public class CompleteRegistrationRequest
{
    public string LicenseKey { get; set; }
    public string Email { get; set; }
}

public class AppSumoWebhookRequest
{
    public string license_key { get; set; }
    public string? prev_license_key { get; set; }
    public string plan_id { get; set; }
    public string @event { get; set; }
    public string license_status { get; set; }
    public long event_timestamp { get; set; }
    public long created_at { get; set; }
    public int? tier { get; set; }
    public bool test { get; set; }
    public WebhookExtra? extra { get; set; }
}

public class WebhookExtra
{
    public string? reason { get; set; }
}