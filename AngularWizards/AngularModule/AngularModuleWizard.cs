﻿using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;
using AngularWizards.Services;
using System.Text.RegularExpressions;

namespace AngularWizards.AngularModule
{
    public class AngularModuleWizard : IWizard
    {
        protected readonly NameService _nameSvc = new NameService();

        public void BeforeOpeningFile(global::EnvDTE.ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(global::EnvDTE.Project project)
        {
        }

        public void ProjectItemFinishedGenerating(global::EnvDTE.ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                var itemName = replacementsDictionary["$rootname$"];
                itemName = Regex.Replace(itemName, @"\W?(module)?(.ts)?$", "", RegexOptions.IgnoreCase);
                if(string.IsNullOrWhiteSpace(itemName))
                {
                    itemName = "my";
                }
                var itemParts = _nameSvc.SplitName(itemName);
                var moduleClassName = $"{_nameSvc.ToPascalCase(itemParts)}Module";
                var moduleFileName = $"{string.Join("-", itemParts)}.module.ts";
                replacementsDictionary.Add($"${nameof(moduleClassName)}$", moduleClassName);
                replacementsDictionary.Add($"${nameof(moduleFileName)}$", moduleFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new WizardCancelledException("Error", ex);
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
