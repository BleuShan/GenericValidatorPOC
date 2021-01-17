using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericValidatorPOC.Core
{
  public interface IValidator<TargetT> where TargetT : new()
  {
    void Validate(TargetT target);
  }
}