using Integgreat.Application.Services;
using Microsoft.Extensions.Configuration;
using Resend;
using System.Net.Mail;

namespace Integgreat.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IResend _resend;
    private readonly string _fromEmail;

    public EmailService(IResend resend, IConfiguration configuration)
    {
        _resend = resend;
        _fromEmail = configuration["Resend:FromEmail"]!;
    }

    public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
    {
        var message = new EmailMessage
        {
            From = _fromEmail,
            To = { toEmail },
            Subject = "Réinitialisation de votre mot de passe — Integgreat",
            HtmlBody = $"""
                <div style="font-family: sans-serif; max-width: 480px; margin: auto; padding: 32px; background: #0f172a; border-radius: 12px;">
                    <h2 style="color: #f8fafc;">Réinitialisation du mot de passe</h2>
                    <p style="color: #94a3b8;">Cliquez sur le bouton ci-dessous pour réinitialiser votre mot de passe. Ce lien expire dans 1 heure.</p>
                    <a href="{resetLink}" 
                       style="display: inline-block; margin: 24px 0; padding: 12px 24px; background: linear-gradient(135deg, #2dd4bf, #7c5cfc); color: white; text-decoration: none; border-radius: 8px; font-weight: 600;">
                        Réinitialiser mon mot de passe
                    </a>
                    <p style="color: #475569; font-size: 12px;">Si vous n'avez pas demandé cette réinitialisation, ignorez ce courriel.</p>
                </div>
            """
        };

        await _resend.EmailSendAsync(message);
    }
}