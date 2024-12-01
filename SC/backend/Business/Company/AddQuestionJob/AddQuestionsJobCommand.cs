﻿using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Company.AddQuestionJob;

public record AddQuestionsJobCommand(AddQuestionCompanyDto Dto) : IRequest<string>;