using Application.Common.DTOs;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TodoListItems.Queries.GetPaginated
{
    #region Request
    public class GetTodoListItemsPaginatedQuery : IRequest<PaginatedList<TodoListItemDto>>
    {
        public string ListId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int ResultsPerPage { get; set; } = 10;
    }
    #endregion

    #region Validator
    public class GetTodoListItemsPaginatedQueryValidator : AbstractValidator<GetTodoListItemsPaginatedQuery>
    {
        public GetTodoListItemsPaginatedQueryValidator()
        {
            RuleFor(v => v.ListId)
                .NotEmpty().WithMessage("Missing the list Id.");

            RuleFor(v => v.PageNumber)
                .GreaterThan(0).WithMessage("Invalid page number.");

            RuleFor(v => v.ResultsPerPage)
                .InclusiveBetween(10, 200).WithMessage("Results per page must be between 10 and 200.");
        }
    }
    #endregion

    #region Handler
    public class GetTodoListItemsPaginatedQueryHandler : IRequestHandler<GetTodoListItemsPaginatedQuery, PaginatedList<TodoListItemDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetTodoListItemsPaginatedQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TodoListItemDto>> Handle(GetTodoListItemsPaginatedQuery request, CancellationToken cancellationToken)
        {
            var paginatedTodoListItems = await _context.TodoListItems
                .AsNoTracking()
                .Where(li => li.CreatedBy == _currentUserService.UserId && li.ListId == request.ListId)
                .OrderBy(li => li.Title)
                .ProjectTo<TodoListItemDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.ResultsPerPage);

            return paginatedTodoListItems;
        }
    }
    #endregion
}
