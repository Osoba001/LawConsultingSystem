﻿using Law.Domain.Models;
using Law.Domain.Repositories;
using SimpleMediatR.MediatRContract;
using Utilities.ActionResponse;
using Utilities.RegexFormatValidations;

namespace Law.Application.Commands.DepartmentC
{
    public record CreateDepartment: ICommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ActionResult Validate()
        {
            var res = new ActionResult();
            if (!Name.StringMaxLength(100))
            {
                res.AddError("Department name is to long.");
            }
            if (!Description.StringMaxLength(200))
            {
                res.AddError("Department description is to long.");
            }
            return res;
        }
    }

    public class CreateDepartmentHandler : ICommandHandler<CreateDepartment>
    {
        public async Task<ActionResult> HandleAsync(CreateDepartment command, IRepoWrapper repo, IServiceProvider ServiceProvider, CancellationToken cancellationToken = default)
        {
            var dept = await repo.DepartmentRepo.FindOneByPredicate(x => x.Name.ToLower() == command.Name.Trim().ToLower());
            if (dept != null)
            {
                return repo.DepartmentRepo.FailedAction("Department already exist!");
            }
            return await repo.DepartmentRepo.Add(new Department(command.Name.Trim(), command.Description));
        }
    }

}
