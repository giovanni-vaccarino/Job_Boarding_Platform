﻿namespace backend.Shared.EmailService;

public interface IEmailService
{
    Task SendEmailAsync(string toAddress, string subject, string body);
}