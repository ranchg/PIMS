using System.Collections.Generic;
using System.Web.Optimization;

namespace SSI.Web
{
    //压缩打包静态资源 By 阮创 2017/11/30
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            bundles.Add(
                new StyleBundle("~/bundles/ssi-css") { Orderer = new AsIsBundleOrderer() }
                    .Include("~/Static/bootstrap/css/bootstrap.min.css", new CssRewriteUrlTransform())
                    .Include("~/Static/bootstrap-table/bootstrap-table.min.css", new CssRewriteUrlTransform())
                    .Include("~/Static/bootstrap-fileinput/css/fileinput.min.css", new CssRewriteUrlTransform())
                    .Include("~/Static/bootstrap-datetimepicker/css/bootstrap-datetimepicker.min.css", new CssRewriteUrlTransform())
                    .Include("~/Static/ace/css/ace.min.css", new CssRewriteUrlTransform())
                    .Include("~/Static/ace/css/ace-skins.min.css", new CssRewriteUrlTransform())
                    .Include("~/Static/ace/css/ace-rtl.min.css", new CssRewriteUrlTransform())
                    //.Include("~/Static/ace/css/ace-part2.min.css", new CssRewriteUrlTransform())
                    //.Include("~/Static/ace/css/ace-ie.min.css", new CssRewriteUrlTransform())
                    .Include("~/Static/layer/theme/default/layer.css", new CssRewriteUrlTransform())
                    .Include("~/Static/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform())
                    .Include("~/Static/select2/css/select2.min.css", new CssRewriteUrlTransform())
                    .Include("~/Static/zTree/css/metroStyle/metroStyle.css", new CssRewriteUrlTransform())
            );

            bundles.Add(
                new ScriptBundle("~/bundles/ssi-ie8") { Orderer = new AsIsBundleOrderer() }
                    .Include(
                        "~/Static/html5-ie8/html5shiv.min.js",
                        "~/Static/html5-ie8/respond.min.js"
                    )
            );

            bundles.Add(
                new ScriptBundle("~/bundles/ssi-js") { Orderer = new AsIsBundleOrderer() }
                    .Include(
                        "~/Static/js.cookie.js",
                        "~/Static/jquery.min.js",
                        "~/Static/jquery.form.min.js",
                        "~/Static/bootstrap/js/bootstrap.min.js",
                        "~/Static/bootstrap-table/bootstrap-table.min.js",
                        "~/Static/bootstrap-table/locale/bootstrap-table-zh-CN.min.js",
                        "~/Static/bootstrap-fileinput/js/fileinput.min.js",
                        "~/Static/bootstrap-fileinput/js/locales/zh.js",
                        "~/Static/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js",
                        "~/Static/bootstrap-datetimepicker/js/locales/bootstrap-datetimepicker.zh-CN.js",
                        "~/Static/ace/js/ace-elements.min.js",
                        "~/Static/ace/js/ace.min.js",
                        "~/Static/jquery-validation/jquery.validate.min.js",
                        "~/Static/jquery-validation/additional-methods.min.js",
                        "~/Static/jquery-validation/localization/messages_zh.min.js",
                        "~/Static/select2/js/select2.full.min.js",
                        "~/Static/select2/js/i18n/zh-CN.js",
                        "~/Static/zTree/js/jquery.ztree.all.min.js"
                    )
            );

            BundleTable.EnableOptimizations = true;
        }
    }
    class AsIsBundleOrderer : IBundleOrderer
    {
        public virtual IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}