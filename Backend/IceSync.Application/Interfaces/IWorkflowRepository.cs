using System;
using IceSync.Domain;

namespace IceSync.Application.Interfaces;

public interface IWorkflowRepository
{
    Task<IList<Workflow>> GetAllByUserId(string userId);
    Task CreateAsync(IList<Workflow> items);
    Task DeleteAsync(IList<Workflow> item);
    Task SaveAsync();
}
