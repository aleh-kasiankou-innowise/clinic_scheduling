using Innowise.Clinic.Scheduling.Persistence.Repositories.Interfaces;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;
using MassTransit;

namespace Innowise.Clinic.Scheduling.Services.MassTransitService.Consumers;

public class DoctorChangesConsumer : IConsumer<DoctorAddedOrUpdatedMessage>
{
    private readonly IDoctorRepository _doctorRepository;

    public DoctorChangesConsumer(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task Consume(ConsumeContext<DoctorAddedOrUpdatedMessage> context)
    {
        await _doctorRepository.AddOrUpdateDoctorAsync(context.Message);
    }
}