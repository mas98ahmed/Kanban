using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Tests
{
    [TestClass]
    class ColumnTests
    {
        Column column;
        Mock<Task> task1;
        Mock<Task> task2;
        Mock<Task> task3;

        [SetUp]
        public void SetUp()
        {
            column = new Column(0, "mas", 6, 0);
            task1 = new Mock<Task>("mas@gmail.com", 0);
            task2 = new Mock<Task>("mas@gmail.com", 1);
            task3 = new Mock<Task>("mas@gmail.com", 2);
        }

        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        [TestCase("column1")]
        [TestCase("")]
        [TestCase(null)]
        public void ChangeNameTest(string name)
        {
            try
            {
                //arrange
                string ExpectedName = name;
                //act
                column.ChangeColumnName(name);
                //assert
                Assert.AreEqual(column.name, ExpectedName, "the changing name is wrong.");
            }
            catch (Exception e)
            {
                if (string.IsNullOrEmpty(name))
                {
                    Assert.AreEqual(e.Message, "The column's name could'nt be empty", "the changing name is wrong.");
                }
                else
                {
                    Assert.AreEqual(e.Message, "The column's name could'nt be more more than 15 letters.", "the changing name is wrong.");
                }
            }
        }


        [TestCase(5)]
        [TestCase(2)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ChangeLimitTest(int limit)
        {
            try
            {
                //arrange
                column.tasks.Add(task1.Object);
                column.tasks.Add(task2.Object);
                column.tasks.Add(task3.Object);
                //act
                column.updatelimit(limit);
                //assert
                Assert.AreEqual(column.limit, limit, "the changing limit is wrong.");
            }
            catch (Exception e)
            {
                if (limit < 0)
                {
                    Assert.AreEqual(e.Message, "The limit is negative.", "the changing limit is wrong.");
                }
                else
                {
                    Assert.AreEqual(e.Message, "The number of th current tasks is higher than the limit.", "the changing limit is wrong.");
                }
            }
        }


        [TestCase(0, "mas@gmail.com")]
        [TestCase(1, "mas@gmail.com")]
        [TestCase(2, "mas1@gmail.com")]
        [TestCase(3, "mas@gmail.com")]
        [TestCase(4, "mas2@gmail.com")]
        public void DeleteTaskTest(int taskId, string emailassignee)
        {
            try
            {
                //arrange
                column.tasks.Add(task1.Object);//0
                column.tasks.Add(task2.Object);//1
                column.tasks.Add(task3.Object);//2
                //act
                column.DeleteTask(emailassignee, taskId);
                //assert
                Assert.AreEqual(column.tasks.Count, 2, "the removing task is wrong.");
            }
            catch (Exception e)
            {
                if (taskId > 2 || taskId < 0)
                {
                    Assert.AreEqual(e.Message, "There is no task with this id.", "the changing delete task is wrong.");
                }
                else
                {
                    Assert.AreEqual(e.Message, "Only the assigned user allowed to delte the task.", "the changing delete task is wrong.");
                }
            }
        }
    }
}
