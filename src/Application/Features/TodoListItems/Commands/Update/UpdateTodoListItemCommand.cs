using Application.Common.Constants;
using Application.Common.DTOs;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TodoListItems.Commands.Update
{
    #region Request
    public class UpdateTodoListItemCommand : IRequest<(Result, TodoListItemDto)>
    {
        public string Id { get; set; }
        public string ListId { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        public bool Done { get; set; }
    }
    #endregion

    #region Validator
    public class UpdateTodoListItemCommandValidator : AbstractValidator<UpdateTodoListItemCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateTodoListItemCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;

            RuleFor(v => v.Id)
                .NotEmpty().WithMessage(ValidationResponses.Required);

            RuleFor(v => v.ListId)
                .NotEmpty().WithMessage(ValidationResponses.Required);

            RuleFor(v => v.Title)
                .NotEmpty().WithMessage(ValidationResponses.Required)
                .MustAsync(BeUniqueTitle).WithMessage("This list already has and item with this title.");
        }

        private async Task<bool> BeUniqueTitle(UpdateTodoListItemCommand request, string title, CancellationToken cancellationToken)
        {
            var hasTitle = await _context.TodoListItems
                .AnyAsync(li => li.ListId == request.ListId && li.CreatedBy == _currentUserService.UserId && li.Id != request.Id, cancellationToken);

            return !hasTitle;
        }
    }
    #endregion

    #region Handler
    public class UpdateTodoListItemCommandHandler : IRequestHandler<UpdateTodoListItemCommand, (Result, TodoListItemDto)>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdateTodoListItemCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<(Result, TodoListItemDto)> Handle(UpdateTodoListItemCommand request, CancellationToken cancellationToken)
        {
            var todoListItem = await _context.TodoListItems
                .FirstOrDefaultAsync(li => li.Id == request.Id && li.ListId == request.ListId && li.CreatedBy == _currentUserService.UserId, cancellationToken);

            if (todoListItem == null)
                throw new NotFoundException(nameof(TodoListItem), request.Id);

            todoListItem.Title = request.Title;
            todoListItem.Notes = request.Notes;
            todoListItem.Done = request.Done;

            _context.TodoListItems.Update(todoListItem);
            await _context.SaveChangesAsync(cancellationToken);

            return (Result.Success(), _mapper.Map<TodoListItemDto>(todoListItem));
        }
    }
    #endregion
}
