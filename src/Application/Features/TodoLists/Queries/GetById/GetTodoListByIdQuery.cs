using Application.Common.Constants;
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

namespace Application.Features.TodoLists.Queries.GetById
{
    #region Request
    public class GetTodoListByIdQuery : IRequest<TodoListDto>
    {
        public string Id { get; set; }
    }
    #endregion

    #region Validator
    public class GetTodoListByIdValidator : AbstractValidator<GetTodoListByIdQuery>
    {
        public GetTodoListByIdValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage(ValidationResponses.Required);
        }
    }
    #endregion

    #region Handler
    public class GetTodoListByIdHandler : IRequestHandler<GetTodoListByIdQuery, TodoListDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetTodoListByIdHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<TodoListDto> Handle(GetTodoListByIdQuery request, CancellationToken cancellationToken)
        {
            var todoList = await _context.TodoLists
                .AsNoTracking()
                .FirstOrDefaultAsync(tl => tl.Id == request.Id && tl.CreatedBy == _currentUserService.UserId, cancellationToken);

            if (todoList == null)
                throw new NotFoundException(nameof(TodoList), request.Id);

            return _mapper.Map<TodoListDto>(todoList);
        }
    }
    #endregion
}
