//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyNewBlog.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Admin
    {
        public int id { get; set; }

        [MinLength(length:4)]
        [AdminValidation]
        public string adminName { get; set; }

        [MinLength(length: 4)]
        public string adminPassword { get; set; }
    }
}
