using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Inspections
{
    public sealed record InspectionPhotoResponse(
        int InspectionPhotoId,
        string PhotoUrl,
        int UploadedByUserId,
        DateTime UploadedAt);
}
