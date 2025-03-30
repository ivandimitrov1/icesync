using System;
using IceSync.Domain;

namespace IceSync.Application.Interfaces;

public interface IWorkflowRepository
{
    Task<IList<Workflow>> GetAllByUserId(string userId);
    Task<Workflow?> GetByIdAsync(int id);

    Task CreateAsync(Workflow item);

    Task CreateAsync(IList<Workflow> items);
    Task UpdateAsync(Workflow item);
    Task DeleteAsync(IList<Workflow> item);
    Task SaveAsync();
}
