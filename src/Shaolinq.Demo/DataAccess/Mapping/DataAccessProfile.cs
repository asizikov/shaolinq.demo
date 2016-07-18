using AutoMapper;
using Shaolinq.Demo.Domain;
using DbAuthor =  Shaolinq.Demo.Model.Author;
using DbPost =  Shaolinq.Demo.Model.Post;

namespace Shaolinq.Demo.DataAccess.Mapping
{
    public class DataAccessProfile : Profile
    {
        public DataAccessProfile()
        {
            CreateMap<DbAuthor, Author>();
            CreateMap<DbPost, Post>();
            CreateMap<Post, DbPost>()
                .ForMember(dest => dest.Author, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
