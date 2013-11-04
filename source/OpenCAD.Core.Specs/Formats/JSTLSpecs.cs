using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using OpenCAD.Core.Formats;
using developwithpassion.specifications.fakeiteasy;

namespace OpenCAD.Core.Specs.Formats
{
    [Subject(typeof(JSTL))]
    public class with_JSTL : Observes<JSTL>
    {

    }
    public class loading_valid_file : with_JSTL
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => sut.LoadFile("Assets/test.jstl"));
        It should_not_cause_exception = () => exception.ShouldBeNull();
    }
    public class loading_invalid_file : with_JSTL
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => sut.LoadFile("DOESNOTEXIST"));
        It should_cause_exception = () => exception.ShouldBeOfType<FileNotFoundException>();
    }

    public class loading_string : with_JSTL
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => sut.Load(@"{""Name"":""Test File"",""Colors"":[{""R"":255,""G"":255,""B"":255,""A"":255},{""R"":0,""G"":255,""B"":255,""A"":255}],""Data"":[{""N"":[0.5,1.5,0.5],""L"":[[1.0001,0.5,8.55555],[0,0,0],[0,0,0]],""C"":0},{""N"":[0.5,1.5,0.5],""L"":[[0,0,0],[0,0,0],[0,0,0]],""C"":1}]}"));
        It should_not_cause_exception = () => exception.ShouldBeNull();
    }

    public class loading_invalid_string : with_JSTL
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => sut.Load(@"{/}"));
        It should_cause_exception = () => exception.ShouldBeOfType<InvalidDataException>();
    }

    public class loading_string_with_missing_name : with_JSTL
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => sut.Load(@"{""Colors"":[{""R"":255,""G"":255,""B"":255,""A"":255},{""R"":0,""G"":255,""B"":255,""A"":255}],""Data"":[{""N"":[0.5,1.5,0.5],""L"":[[0,0,0],[0,0,0],[0,0,0]],""C"":0},{""N"":[0.5,1.5,0.5],""L"":[[0,0,0],[0,0,0],[0,0,0]],""C"":1}]}"));
        It should_cause_exception = () => exception.ShouldBeOfType<ArgumentException>();
    }

    public class loading_string_with_empty_name : with_JSTL
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => sut.Load(@"{""Name"":"""",""Colors"":[{""R"":255,""G"":255,""B"":255,""A"":255},{""R"":0,""G"":255,""B"":255,""A"":255}],""Data"":[{""N"":[0.5,1.5,0.5],""L"":[[0,0,0],[0,0,0],[0,0,0]],""C"":0},{""N"":[0.5,1.5,0.5],""L"":[[0,0,0],[0,0,0],[0,0,0]],""C"":1}]}"));
        It should_not_cause_exception = () => exception.ShouldBeNull();
    }

    public class loading_string_with_missing_colors : with_JSTL
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => sut.Load(@"{""Name"":""Test File"",""Data"":[{""N"":[0.5,1.5,0.5],""L"":[[0,0,0],[0,0,0],[0,0,0]],""C"":0},{""N"":[0.5,1.5,0.5],""L"":[[0,0,0],[0,0,0],[0,0,0]],""C"":1}]}"));
        It should_cause_exception = () => exception.ShouldBeOfType<ArgumentException>();
    }
    public class loading_string_with_empty_colors : with_JSTL
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => sut.Load(@"{""Name"":""Test File"",""Colors"":[],""Data"":[{""N"":[0.5,1.5,0.5],""L"":[[0,0,0],[0,0,0],[0,0,0]],""C"":0},{""N"":[0.5,1.5,0.5],""L"":[[0,0,0],[0,0,0],[0,0,0]],""C"":1}]}"));
        It should_need_atleast_one = () => exception.ShouldBeOfType<ArgumentException>();
    }

    public class loading_string_with_invalid_color_ref : with_JSTL
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => sut.Load(@"{""Name"":""Test File"",""Colors"":[{""R"":255,""G"":255,""B"":255,""A"":255},{""R"":0,""G"":255,""B"":255,""A"":255}],""Data"":[{""N"":[0.5,1.5,0.5],""L"":[[1.0001,0.5,8.55555],[0,0,0],[0,0,0]],""C"":0},{""N"":[0.5,1.5,0.5],""L"":[[0,0,0],[0,0,0],[0,0,0]],""C"":2}]}"));
        It should_need_atleast_one = () => exception.ShouldBeOfType<ArgumentException>();
    }

    public class loading_string_with_missing_data : with_JSTL
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => sut.Load(@"{""Name"":""Test File"",""Colors"":[{""R"":255,""G"":255,""B"":255,""A"":255},{""R"":0,""G"":255,""B"":255,""A"":255}]}"));
        It should_cause_exception = () => exception.ShouldBeOfType<ArgumentException>();
    }

    public class loading_string_with_empty_data : with_JSTL
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => sut.Load(@"{""Name"":""Test File"",""Colors"":[{""R"":255,""G"":255,""B"":255,""A"":255},{""R"":0,""G"":255,""B"":255,""A"":255}],""Data"":[]}"));
        It should_not_cause_exception = () => exception.ShouldBeNull();
    }

    public class loading_string_with_normal_too_long : with_JSTL
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => sut.Load(@"{""Name"":""Test File"",""Colors"":[{""R"":255,""G"":255,""B"":255,""A"":255},{""R"":0,""G"":255,""B"":255,""A"":255}],""Data"":[{""N"":[0.5,1.5,0.5,0.5],""L"":[[0,0,0],[0,0,0],[0,0,0]],""C"":1}]}"));
        It should_cause_exception = () => exception.ShouldBeOfType<ArgumentException>();
    }

    public class loading_string_with_normal_too_short : with_JSTL
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => sut.Load(@"{""Name"":""Test File"",""Colors"":[{""R"":255,""G"":255,""B"":255,""A"":255},{""R"":0,""G"":255,""B"":255,""A"":255}],""Data"":[{""N"":[0.5,1.5],""L"":[[0,0,0],[0,0,0],[0,0,0]],""C"":1}]}"));
        It should_cause_exception = () => exception.ShouldBeOfType<ArgumentException>();
    }

    public class loading_string_with_loop_too_long : with_JSTL
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => sut.Load(@"{""Name"":""Test File"",""Colors"":[{""R"":255,""G"":255,""B"":255,""A"":255},{""R"":0,""G"":255,""B"":255,""A"":255}],""Data"":[{""N"":[0.5,1.5,0.5],""L"":[[0,0,0],[0,0,0],[0,0,0],[0,0,1.0]],""C"":1}]}"));
        It should_cause_exception = () => exception.ShouldBeOfType<ArgumentException>();
    }

    public class loading_string_with_loop_too_short : with_JSTL
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => sut.Load(@"{""Name"":""Test File"",""Colors"":[{""R"":255,""G"":255,""B"":255,""A"":255},{""R"":0,""G"":255,""B"":255,""A"":255}],""Data"":[{""N"":[0.5,1.5,0.5],""L"":[[0,0,0],[0,0,0]],""C"":1}]}"));
        It should_cause_exception = () => exception.ShouldBeOfType<ArgumentException>();
    }
}
