using Application.Common.DTOs;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TodoListItems.Queries.GetById
{
    #region Request
    public class GetTodoListItemByIdQuery : IRequest<TodoListItemDto>
    {
        public string ListId { get; set; }
        public string Id { get; set; }
    }
    #endregion

    #region Validator
    public class GetTodoListItemByIdQueryValidator : AbstractValidator<GetTodoListItemByIdQuery>
    {
        public GetTodoListItemByIdQueryValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("The item Id is required.");

            RuleFor(v => v.ListId)
                .NotEmpty().WithMessage("The list Id is required.");
        }
    }
    #endregion

    #region Handler
    public class GetTodoListItemByIdQueryHandler : IRequestHandler<GetTodoListItemByIdQuery, TodoListItemDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetTodoListItemByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<TodoListItemDto> Handle(GetTodoListItemByIdQuery request, CancellationToken cancellationToken)
        {
            var todoListItem = await _context.TodoListItems
                .FirstOrDefaultAsync(li => li.Id == request.Id && li.ListId == request.ListId && li.CreatedBy == _currentUserService.UserId, cancellationToken);

            if (todoListItem == null)
                throw new NotFoundException(nameof(TodoListItem), request.Id);

            return _mapper.Map<TodoListItemDto>(todoListItem);
        }
    }
    #endregion
}
