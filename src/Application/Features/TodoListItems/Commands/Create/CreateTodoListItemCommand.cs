using Application.Common.Constants;
using Application.Common.DTOs;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TodoListItems.Commands.Create
{
    #region Request
    public class CreateTodoListItemCommand : IRequest<(Result, TodoListItemDto)>
    {
        public string ListId { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
    }
    #endregion

    #region Validator
    public class CreateTodoListItemCommandValidator : AbstractValidator<CreateTodoListItemCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateTodoListItemCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;

            RuleFor(v => v.ListId)
                .NotEmpty().WithMessage(ValidationResponses.Required);

            RuleFor(v => v.Title)
                .NotEmpty().WithMessage(ValidationResponses.Required)
                .MustAsync(BeUniqueTitle).WithMessage("This list already contains an item with matching title.");
        }

        private async Task<bool> BeUniqueTitle(CreateTodoListItemCommand request, string title, CancellationToken cancellationToken)
        {
            var containsTitle = await _context.TodoListItems
                .AnyAsync(li => li.ListId == request.ListId && li.CreatedBy == _currentUserService.UserId && li.Title == title, cancellationToken);

            return !containsTitle;
        }
    }
    #endregion

    #region Handler
    public class CreateTodoListItemCommandHandler : IRequestHandler<CreateTodoListItemCommand, (Result, TodoListItemDto)>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public CreateTodoListItemCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<(Result, TodoListItemDto)> Handle(CreateTodoListItemCommand request, CancellationToken cancellationToken)
        {
            var ownsList = await _context.TodoLists
                .AnyAsync(l => l.Id == request.ListId && l.CreatedBy == _currentUserService.UserId, cancellationToken);

            if (!ownsList)
                throw new UnauthorizedAccessException();

            var todoListItem = new TodoListItem { ListId = request.ListId, Title = request.Title, Notes = request.Notes };

            await _context.TodoListItems.AddAsync(todoListItem, cancellationToken);

            return (Result.Success(), _mapper.Map<TodoListItemDto>(todoListItem));
        }
    }
    #endregion
}
