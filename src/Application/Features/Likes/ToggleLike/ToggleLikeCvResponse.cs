namespace Application.Features.Likes.ToggleLike;

public record ToggleLikeCvResponse(
    bool IsLiked,
    int LikesCount
);