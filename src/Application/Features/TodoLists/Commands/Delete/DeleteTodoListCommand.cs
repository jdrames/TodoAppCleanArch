using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TodoLists.Commands.Delete
{
    #region Request
    public class DeleteTodoListCommand : IRequest<Result>
    {
        public string Id { get; set; }
    }
    #endregion

    #region Validator
    public class DeleteTodoListCommandValidator : AbstractValidator<DeleteTodoListCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteTodoListCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;

            RuleFor(v => v.Id)
                .NotEmpty().WithMessage(ValidationResponses.Required);
        }        
    }
    #endregion

    #region Handler
    public class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteTodoListCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _context.TodoLists
                .FirstOrDefaultAsync(tl => tl.Id == request.Id && tl.CreatedBy == _currentUserService.UserId, cancellationToken);

            if (todoList == null)
                throw new NotFoundException(nameof(TodoList), request.Id);

            _context.TodoLists.Remove(todoList);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
    #endregion
}
