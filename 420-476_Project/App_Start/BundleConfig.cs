﻿using System.Web;
using System.Web.Optimization;

namespace _420_476_Project
{
    public class BundleConfig
    {
        // Pour plus d'informations sur le regroupement, visitez http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
             "~/Content/themes/base/core.css",
             "~/Content/themes/base/resizable.css",
             "~/Content/themes/base/selectable.css",
             "~/Content/themes/base/accordion.css",
             "~/Content/themes/base/autocomplete.css",
             "~/Content/themes/base/button.css",
             "~/Content/themes/base/dialog.css",
             "~/Content/themes/base/slider.css",
             "~/Content/themes/base/tabs.css",
             "~/Content/themes/base/datepicker.css",
             "~/Content/themes/base/progressbar.css",
             "~/Content/themes/base/theme.css"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilisez la version de développement de Modernizr pour le développement et l'apprentissage. Puis, une fois
            // prêt pour la production, utilisez l'outil de génération (bluid) sur http://modernizr.com pour choisir uniquement les tests dont vous avez besoin.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
