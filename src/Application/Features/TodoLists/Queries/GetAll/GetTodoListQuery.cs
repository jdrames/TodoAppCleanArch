using Application.Common.DTOs;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TodoLists.Queries.GetAll
{
    #region Request
    public class GetTodoListQuery : IRequest<IList<TodoListDto>>
    {

    }
    #endregion

    #region Handler
    public class GetTodoListQueryHandler : IRequestHandler<GetTodoListQuery, IList<TodoListDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetTodoListQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<IList<TodoListDto>> Handle(GetTodoListQuery request, CancellationToken cancellationToken)
        {
            var todoLists = await _context.TodoLists
                .AsNoTracking()
                .Where(tl => tl.CreatedBy == _currentUserService.UserId)
                .OrderByDescending(tl => tl.Priority)
                .ThenBy(tl => tl.Title)
                .ProjectTo<TodoListDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return todoLists;
        }
    }
    #endregion
}
