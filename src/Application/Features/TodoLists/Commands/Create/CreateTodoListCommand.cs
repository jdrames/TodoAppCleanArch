using Application.Common.Constants;
using Application.Common.DTOs;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TodoLists.Commands.Create
{
    #region Request
    public class CreateTodoListCommand : IRequest<(Result, TodoListDto)>
    {
        public string Title { get; set; }
        public PriorityLevel Priority { get; set; } = default(PriorityLevel);
    }
    #endregion

    #region Validator
    public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateTodoListCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;

            RuleFor(v => v.Title)
                .NotEmpty().WithMessage(ValidationResponses.Required)
                .MinimumLength(6).WithMessage(ValidationResponses.MinimumLength(6))
                .MustAsync(BeUniqueTitle).WithMessage("You already have a todo list with that title.");
        }

        private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            var titleExists = await _context.TodoLists.AnyAsync(l => l.Title == title && l.CreatedBy == _currentUserService.UserId, cancellationToken);
            return !titleExists;
        }
    }
    #endregion

    #region Handler
    public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, (Result, TodoListDto)>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateTodoListCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(Result, TodoListDto)> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
        {
            var todoList = new TodoList() { Title = request.Title, Priority = request.Priority };
            await _context.TodoLists.AddAsync(todoList, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return (Result.Success(), _mapper.Map<TodoListDto>(todoList));
        }
    }
    #endregion
}
