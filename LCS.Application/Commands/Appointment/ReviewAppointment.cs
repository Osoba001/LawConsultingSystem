﻿using LCS.Domain.Repositories;
using LCS.Domain.Response;
using SimpleMediatR.MediatRContract;

namespace LCS.Application.Handlers.Appointment
{
    public record ReviewAppointment(Guid AppointmentId, string Report) : ICommand;

    public class ReviewAppointmentHandler : ICommandHandler<ReviewAppointment>
    {
        public async Task<ActionResult> HandleAsync(ReviewAppointment command, IRepoWrapper repo, CancellationToken cancellationToken = default)
        {
            var appointment = await repo.AppointmentRepo.GetById(command.AppointmentId);
            if (appointment != null)
            {
                appointment.HasReviewed = true;
                appointment.LawyerReport = command.Report;
                return await repo.AppointmentRepo.Update(appointment);
            }
            else
                return repo.FailedAction("Record not found.");
        }
    }
}
