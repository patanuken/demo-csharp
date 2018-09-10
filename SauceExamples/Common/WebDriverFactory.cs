﻿using System;
using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Common
{
    public class WebDriverFactory
    {
        public IWebDriver CreateSauceDriver(string testCaseName)
        {
            var capabilities = new DesiredCapabilities();
            capabilities.SetCapability(CapabilityType.BrowserName, "chrome");
            //will run on the latest browserVersion of the browser
            capabilities.SetCapability(CapabilityType.Version, "latest");
            capabilities.SetCapability(CapabilityType.Platform, "Windows 10");

            return SetSauceCapabilities(testCaseName, capabilities);
        }

        private static IWebDriver SetSauceCapabilities(string testCaseName, DesiredCapabilities capabilities)
        {
            capabilities.SetCapability("username", SauceUser.Name);
            capabilities.SetCapability("accessKey", SauceUser.AccessKey);
            
            //CUSTOM SAUCE CAPABILITIES
            //capabilities.SetCapability("extendedDebugging", true);
            capabilities.SetCapability("build", $"SauceExamples-{DateTime.Now.ToString(CultureInfo.InvariantCulture)}");
            capabilities.SetCapability("captureHtml", true);
            capabilities.SetCapability("tunnelIdentifier", "NikolaysTunnel");
            //How long is a test allowed to run?
            capabilities.SetCapability("maxDuration", 3600);
            //Selenium crash might hang a command, this is the max time allowed to wait for a Selenium command
            capabilities.SetCapability("commandTimeout", 600);
            //How long can the browser wait before a new command?
            capabilities.SetCapability("idleTimeout", 1000);
            var driver = new RemoteWebDriver(new Uri("http://ondemand.saucelabs.com:80/wd/hub"),
                capabilities, TimeSpan.FromSeconds(600));
            ((IJavaScriptExecutor)driver).ExecuteScript($"sauce:job-name={testCaseName}");
            return driver;
        }

        public IWebDriver CreateSauceDriver(string browser, string browserVersion, string osPlatform)
        {
            var capabilities = new DesiredCapabilities();
            capabilities.SetCapability(CapabilityType.BrowserName, browser);
            capabilities.SetCapability(CapabilityType.Version, browserVersion);
            capabilities.SetCapability(CapabilityType.Platform, osPlatform);

            return SetSauceCapabilities(capabilities);
        }

        private IWebDriver SetSauceCapabilities(DesiredCapabilities capabilities)
        {
            return SetSauceCapabilities("", capabilities);
        }
    }
}
