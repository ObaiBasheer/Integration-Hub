using System;

namespace Integration_Hub.Services
{
    public interface IIntegrationService
    {
        Task ExecuteIntegration(Guid integrationId);
    }
}
