using AutoMapper;

namespace Application.Common.Mappings
{
    public interface IMapFrom<T>
    {
        // If not overriden this will be called to do generic mapping
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
