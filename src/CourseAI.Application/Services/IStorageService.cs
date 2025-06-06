﻿namespace CourseAI.Application.Services;

public interface IStorageService
{
    Task<string> SaveImageAsync(string base64Image, string fileName, string path);
    Task DeleteImageAsync(string fileName);
    
    Task<string> SaveVideoAsync(byte[] videoBytes, string fileName, string path);
}