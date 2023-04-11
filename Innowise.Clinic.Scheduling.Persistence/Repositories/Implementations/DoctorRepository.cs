using System.Linq.Expressions;
using Innowise.Clinic.Scheduling.Persistence.Models;
using Innowise.Clinic.Scheduling.Persistence.Repositories.Interfaces;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;
using Microsoft.EntityFrameworkCore;

namespace Innowise.Clinic.Scheduling.Persistence.Repositories.Implementations;

public class DoctorRepository : IDoctorRepository
{
    private readonly SchedulingDbContext _dbContext;

    public DoctorRepository(SchedulingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Doctor?> GetDoctorAsync(Expression<Func<Doctor, bool>> filter)
    {
        return await _dbContext.Doctors.FirstOrDefaultAsync(filter);
    }

    public async Task AddOrUpdateDoctorAsync(DoctorAddedOrUpdatedMessage doctorAddedOrUpdatedMessage)
    {
        var savedDoctor = await GetDoctorAsync(x => x.DoctorId == doctorAddedOrUpdatedMessage.DoctorId);
        if (savedDoctor is null)
        {
            await _dbContext.Doctors.AddAsync(new()
            {
                DoctorId = doctorAddedOrUpdatedMessage.DoctorId,
                SpecializationId = doctorAddedOrUpdatedMessage.SpecializationId,
                OfficeId = doctorAddedOrUpdatedMessage.OfficeId
            });
        }

        else
        {
            savedDoctor.DoctorId = doctorAddedOrUpdatedMessage.DoctorId;
            savedDoctor.OfficeId = doctorAddedOrUpdatedMessage.OfficeId;
            savedDoctor.SpecializationId = doctorAddedOrUpdatedMessage.SpecializationId;
            _dbContext.Doctors.Update(savedDoctor);
        }

        await _dbContext.SaveChangesAsync();
    }
}