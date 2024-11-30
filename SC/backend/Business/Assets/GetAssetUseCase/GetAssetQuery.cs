using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace backend.Business.Assets.GetAssetUseCase;

public record GetAssetQuery(int StudentId, int? CompanyId) : IRequest<FileStreamResult>;