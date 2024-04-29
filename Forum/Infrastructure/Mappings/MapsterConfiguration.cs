using Forum.Application.Comments.Responses;
using Mapster;
using Forum.Domain.Entities;
using Forum.Application.Comments.Requests;
using Forum.Application.Topics.Responses;
using Forum.Application.Topics.Requests;
using Forum.Application.Accounts.Requests;
using Forum.Application.Accounts.Responses;

namespace Forum.API.Infrastructure.Mappings
{
    public static class MapsterConfiguration
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
            TypeAdapterConfig<Topic, TopicResponseModel>.NewConfig();
            TypeAdapterConfig<Topic, AdminTopicResponseModel>.NewConfig();
            TypeAdapterConfig<TopicRequestPostModel, Topic>.NewConfig();
            TypeAdapterConfig<TopicRequestPutModel, Topic>.NewConfig();
            TypeAdapterConfig<TopicResponseModel, TopicRequestPutModel>.NewConfig();
            TypeAdapterConfig<Comment, CommentResponseModel>.NewConfig();
            TypeAdapterConfig<CommentRequestPostModel, Comment>.NewConfig();
            TypeAdapterConfig<RegistrationRequestModel, User>.NewConfig();
            TypeAdapterConfig<User, AdminResponseModel>.NewConfig();
            TypeAdapterConfig<User, UserPutModel>.NewConfig();

        }
    }
}
