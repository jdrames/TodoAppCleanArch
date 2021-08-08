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

namespace Application.Features.TodoListItems.Commands.Delete
{
    #region Request
    public class DeleteTodoListItemCommand : IRequest<Result>
    {
        public string Id { get; set; }
        public string ListId { get; set; }
    }
    #endregion

    #region Validator
    public class DeleteTodoListItemCommandValidator : AbstractValidator<DeleteTodoListItemCommand>
    {
        public DeleteTodoListItemCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage(ValidationResponses.Required);

            RuleFor(v => v.ListId)
                .NotEmpty().WithMessage(ValidationResponses.Required);
        }
    }
    #endregion

    #region Handler
    public class DeleteTodoListItemCommandHandler : IRequestHandler<DeleteTodoListItemCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteTodoListItemCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(DeleteTodoListItemCommand request, CancellationToken cancellationToken)
        {
            var todoListItem = await _context.TodoListItems
                .FirstOrDefaultAsync(li => li.ListId == request.ListId && li.Id == request.Id && li.CreatedBy == _currentUserService.UserId);

            if (todoListItem == null)
                throw new NotFoundException(nameof(TodoListItem), request.Id);

            _context.TodoListItems.Remove(todoListItem);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
    #endregion
}
