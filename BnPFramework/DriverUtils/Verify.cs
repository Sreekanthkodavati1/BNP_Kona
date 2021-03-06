﻿namespace BnPBaseFramework.Web
{
    using System;
    using System.Globalization;

    using NLog;

    using BnPBaseFramework.Web.Types;

    public static class Verify
    {
        private static readonly NLog.Logger Logger = LogManager.GetLogger("TEST");

        /// <summary>
        /// Verify group of assets
        /// </summary>
        /// <param name="driverContext">Container for driver</param>
        /// <param name="myAsserts">Group asserts</param>
        /// <example>How to use it: <code>
        /// Verify.That(
        ///     this.DriverContext,
        ///     () => Assert.AreEqual(5 + 7 + 2, forgotPassword.EnterEmail(5, 7, 2)),
        ///     () => Assert.AreEqual("Your e-mail's been sent!", forgotPassword.ClickRetrievePassword));
        /// </code></example>
        public static void That(DriverContext driverContext, params Action[] myAsserts)
        {
            That(driverContext, false, false, myAsserts);
        }

        /// <summary>
        /// Verify group of assets
        /// </summary>
        /// <param name="driverContext">Container for driver</param>
        /// <param name="enableScreenShot">Enable screenshot</param>
        /// <param name="enableSavePageSource">Enable save page source</param>
        /// <param name="myAsserts">Group asserts</param>
        /// <example>How to use it: <code>
        /// Verify.That(
        ///     this.DriverContext,
        ///     true,
        ///     false,
        ///     () => Assert.AreEqual(5 + 7 + 2, forgotPassword.EnterEmail(5, 7, 2)),
        ///     () => Assert.AreEqual("Your e-mail's been sent!", forgotPassword.ClickRetrievePassword));
        /// </code></example>
        public static void That(DriverContext driverContext, bool enableScreenShot, bool enableSavePageSource, params Action[] myAsserts)
        {
            foreach (var myAssert in myAsserts)
            {
                That(driverContext, myAssert, false, false);
            }

            if (!driverContext.VerifyMessages.Count.Equals(0) && enableScreenShot)
            {
                driverContext.TakeAndSaveScreenshot();
            }

            if (!driverContext.VerifyMessages.Count.Equals(0) && enableSavePageSource)
            {
                driverContext.SavePageSource(driverContext.TestTitle);
            }
        }

        /// <summary>
        /// Verify assert conditions
        /// </summary>
        /// <param name="driverContext">Container for driver</param>
        /// <param name="myAssert">Assert condition</param>
        /// <param name="enableScreenShot">Enabling screenshot</param>
        /// <param name="enableSavePageSource">Enable save page source</param>
        /// <example>How to use it: <code>
        /// Verify.That(this.DriverContext, () => Assert.IsFalse(flag), true);
        /// </code></example>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "removed ref to unit test")]
        public static void That(DriverContext driverContext, Action myAssert, bool enableScreenShot, bool enableSavePageSource)
        {
            try
            {
                myAssert();
            }
            catch (Exception e)
            {
                if (enableScreenShot)
                {
                    driverContext.TakeAndSaveScreenshot();
                }

                if (enableSavePageSource)
                {
                    driverContext.SavePageSource(driverContext.TestTitle);
                }

                driverContext.VerifyMessages.Add(new ErrorDetail(null, DateTime.Now, e));

                Logger.Error(CultureInfo.CurrentCulture, "<VERIFY FAILS>\n{0}\n</VERIFY FAILS>", e);
            }
        }

        /// <summary>
        /// Verify assert conditions
        /// </summary>
        /// <param name="driverContext">Container for driver</param>
        /// <param name="myAssert">Assert condition</param>
        /// <example>How to use it: <code>
        /// Verify.That(this.DriverContext, () => Assert.AreEqual(parameters["number"], links.CountLinks()));
        /// </code></example>
        public static void That(DriverContext driverContext, Action myAssert)
        {
            That(driverContext, myAssert, false, false);
        }
    }
}
