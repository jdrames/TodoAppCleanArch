using Application.Common.Constants;
using Application.Common.DTOs;
using Application.Common.Exceptions;
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

namespace Application.Features.TodoLists.Commands.Update
{
    #region Request
    public class UpdateTodoListCommand : IRequest<(Result, TodoListDto)>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public PriorityLevel Priority { get; set; }
    }
    #endregion

    #region Validator
    public class UpdateTodoListCommandValidator : AbstractValidator<UpdateTodoListCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateTodoListCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;

            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Missing List Id.");

            RuleFor(v => v.Title)
                .NotEmpty().WithMessage(ValidationResponses.Required)
                .MinimumLength(6).WithMessage(ValidationResponses.MinimumLength(6))
                .MustAsync(BeUniqueTitle).WithMessage("You already have a todo list with that title.");
        }

        private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            var titleExists = await _context.TodoLists.AnyAsync(l => l.Title == title && l.CreatedBy == _currentUserService.UserId);
            return !titleExists;
        }
    }
    #endregion

    #region Handler
    public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand, (Result, TodoListDto)>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UpdateTodoListCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<(Result, TodoListDto)> Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _context.TodoLists
                .FirstOrDefaultAsync(tl => tl.CreatedBy == _currentUserService.UserId && tl.Id == request.Id, cancellationToken);
            
            if (todoList == null)
                throw new NotFoundException(nameof(TodoList), request.Id);

            todoList.Title = request.Title;
            todoList.Priority = request.Priority;

            await _context.SaveChangesAsync(cancellationToken);

            return (Result.Success(), _mapper.Map<TodoListDto>(todoList));
        }
    }
    #endregion
}
