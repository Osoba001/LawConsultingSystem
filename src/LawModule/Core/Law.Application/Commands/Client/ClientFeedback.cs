﻿using Law.Domain.Constants;
using Law.Domain.Repositories;
using SimpleMediatR.MediatRContract;
using Utilities.ActionResponse;
using Utilities.RegexFormatValidations;

namespace Law.Application.Commands.ClientC
{
    public record ClientFeedback: ICommand
    {
        public Guid AppointmentId { get; set; }
        public string Feedback { get; set; }
        public Star Star { get; set; }
        public ActionResult Validate()
        {
            ActionResult res = new();
            if (!Feedback.StringMaxLength(500))
            {
                res.AddError("Feedback message is to long.");
            }
            return res;

        }
    }

    public class ClientFeedbackHandler : ICommandHandler<ClientFeedback>
    {
        public async Task<ActionResult> HandleAsync(ClientFeedback command, IRepoWrapper repo, IServiceProvider ServiceProvider, CancellationToken cancellationToken = default)
        {
            var appointment = await repo.AppointmentRepo.GetById(command.AppointmentId);
            if (appointment != null)
            {
                appointment.Stars = command.Star;
                appointment.ClientFeedBack = command.Feedback;
                return await repo.AppointmentRepo.Update(appointment);
            }
            else
                return repo.FailedAction("Record not found.");
        }
    }
}
