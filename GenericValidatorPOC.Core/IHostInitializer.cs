﻿using System;

namespace GenericValidatorPOC.Core
{
  public interface IHostInitializer

  {
    public void Configure(ValidatorProvider provider);
  }
}