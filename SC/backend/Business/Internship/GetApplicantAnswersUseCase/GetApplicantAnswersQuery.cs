﻿using backend.Service.Contracts.Internship;
using MediatR;

namespace backend.Business.Internship.GetApplicantAnswersUseCase;

public record GetApplicantAnswersQuery(int ApplicationId, int StudentId) : IRequest<ApplicantDetailsResponse>;