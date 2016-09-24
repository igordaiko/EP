using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using EnerProf.DataBaseClasses;
using EnerProf.Models;
namespace EnerProf.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<CodeContext>());

            using (var context = new CodeContext())
            {
                //Company company = new Company { Name = "Enerpac" };
                Category cat1 = new Category { Img = "/images/cat1.png", Name = "Цилиндры", IsOnStartPage = true, Parent = null, Attributes = new List<AttributeModel>() };
                //Category cat2 = new Category { Name = "Алюминиевые", Img = "/images/gidraulic.jpg", IsOnStartPage = false, Parent = cat1 };
                //context.Categories.Add(cat2);
                //Product product1 = new Product
                //{
                //    Name = "RC-цилиндр",
                //    Img = "/images/gidraulic.jpg",
                //    Description = "Гидравлические цилиндры Enerpac имеют сотни различных конфигураций. Какой бы ни была Ваша задача: подъем или удержание груза, изгиб и т.д., какие бы ни требовались размеры, усилие, ход штока, нужен ли Вам цилиндр гидравлический одностороннего действия или двустороннего, будьте уверены: наши цилиндры подойдут для любых Ваших целей.",
                //    Images = "/images/gidraulic.jpg;/images/cat1.png;/images/cat1.png",
                //    Category = cat2,
                //    Company = company
                //};
                //Product product2 = new Product
                //{
                //    Name = "RB-цилиндр",
                //    Img = "/images/gidraulic.jpg",
                //    Description = "Гидравлические цилиндры Enerpac имеют сотни различных конфигураций. Какой бы ни была Ваша задача: подъем или удержание груза, изгиб и т.д., какие бы ни требовались размеры, усилие, ход штока, нужен ли Вам цилиндр гидравлический одностороннего действия или двустороннего, будьте уверены: наши цилиндры подойдут для любых Ваших целей.",
                //    Images = "/images/gidraulic.jpg;/images/cat1.png;/images/cat1.png",
                //    Category = cat2,
                //    Company = company
                //};

                //Model model1 = new Model { Name = "RC-1000", Product = product1 };
                //Model model2 = new Model { Name = "RC-2000", Product = product1 };
                //Model model3 = new Model { Name = "RB-1000", Product = product2 };
                //Model model4 = new Model { Name = "RB-2000", Product = product2 };
                ////context.Products.Add(product1);
                //Unit unit1 = new Unit { Name = "мм" };

                //AttributeModel attr1 = new AttributeModel() { Categories = new List<Category>(), Name = "Ширина", Status = true, Type = Types.Double, Unit = unit1 };

                //AttributeModel attr2 = new AttributeModel() { Categories = new List<Category>(), Name = "Радиус", Status = true, Type = Types.Double, Unit = unit1 };
                //AttributeModel attr3 = new AttributeModel() { Category = cat2, Name = "Производитель", Status = true, Type = Types.String, };


                //context.AttributesValues.Add(new AttributesValues { Attribute = attr1, Model = model1, Double_Value = 20 });
                //context.AttributesValues.Add(new AttributesValues { Attribute = attr1, Model = model2, Double_Value = 30 });
                //context.AttributesValues.Add(new AttributesValues { Attribute = attr1, Model = model3, Double_Value = 40 });
                //context.AttributesValues.Add(new AttributesValues { Attribute = attr1, Model = model4, Double_Value = 50 });

                //context.AttributesValues.Add(new AttributesValues { Attribute = attr2, Model = model1, Double_Value = 50 });
                //context.AttributesValues.Add(new AttributesValues { Attribute = attr2, Model = model2, Double_Value = 100 });
                //context.AttributesValues.Add(new AttributesValues { Attribute = attr2, Model = model3, Double_Value = 70 });
                //context.AttributesValues.Add(new AttributesValues { Attribute = attr2, Model = model4, Double_Value = 120 });

                //context.AttributesValues.Add(new AttributesValues { Attribute = attr3, Model = model1, String_Value = "Enerpac" });
                //context.AttributesValues.Add(new AttributesValues { Attribute = attr3, Model = model2, String_Value = "Enerpac" });
                //context.AttributesValues.Add(new AttributesValues { Attribute = attr3, Model = model3, String_Value = "WallMag" });
                //context.AttributesValues.Add(new AttributesValues { Attribute = attr3, Model = model4, String_Value = "WallMag" });
                //cat1.Attributes.Add(attr1);

                //context.Categories.Add(cat1);
                //context.Attributes.Add(attr2);
                //context.SaveChanges();
                //cat1 = context.Categories.Find(cat1.Id);
                //cat1.Attributes.Add(attr2);
                //context.SaveChanges();


                IEnumerable<Category> model_categories = context.Categories.ToList();

                foreach (var category in model_categories)
                    category.Sub = model_categories.Where(cat => cat.Parent == category).ToList();

                ViewBag.ListOfCategories = model_categories;
                ViewBag.ListOfIndustries = context.Industries.ToList();
                ViewBag.Clients = context.Clients;

                ViewBag.Title = "Enerprof";
            }

            return View();
        }

        public ActionResult categories(int? parent)
        {
            Category model;
            List<Category> cats = new List<Category>();
            using (var context = new CodeContext())
            {
                context.Categories.ToList();
                if (parent == null)
                {
                    model = new Category();
                    cats = context.Categories.Where(parent_cat => parent_cat.Parent == null).ToList();
                    ViewBag.CatName = "Оборудование";
                }
                else
                {
                    model = context.Categories.Where(cat => cat.Id == parent).First();
                    ViewBag.CatName = context.Categories.Where(cat => cat.Id == parent).Select(name => name.Name).First();
                    model.Description = context.Categories.Where(cat => cat.Id == parent).Select(desc => desc.Description).First();
                    cats = context.Categories.Where(cat => cat.Parent.Id == parent).ToList();
                }

                model.Sub = cats;
                if (cats.Count == 0)
                    return products((int)parent, null);
            }


            return View(model);
        }

        public PartialViewResult filters(int cat_id)
        {

            List<AttributesValues> attributes = null;
            using (var context = new CodeContext())
            {
                context.Attributes.ToList();
                context.Products.ToList();
                Category cat = context.Categories.First(c => c.Id == cat_id);
                attributes = context.AttributesValues
                    .Where(attr => (attr.Attribute.Categories.Where(c => c.Id == cat_id) != null
                || attr.Attribute.Categories.Where(c => c.Id == context.Categories.Find(cat_id).Parent.Id) != null) && attr.Attribute.Status == true)
                    .ToList();


            }
            ViewBag.CatID = cat_id;
            return PartialView(attributes);
        }

        public ActionResult products(int cat_id, Category cat_model)
        {
            Category model = new Category();
            var context = new CodeContext();
            context.Categories.ToArray();
            model = context.Categories.Find(cat_id);
            int parent_id = 0;
            if (model.Parent != null)
                parent_id = model.Parent.Id;

            if (model.Products == null)
                model.Products = context.Products.Where(p => p.Category.Id == cat_id).ToList();
            context.Attributes.ToList();
            context.Products.ToList();
            Category cat = context.Categories.First(c => c.Id == cat_id);
            model.Attributes = context.Categories.Where(c => c.Id == model.Id || c.Id == parent_id)
                                    .Select(c => c.Attributes).ToList().SelectMany(x => x).Distinct().ToList();
            List<Filters> filters = new List<Filters>();
                Filters filter;
            var attributes = context.Models.Where(m => m.Product.Category.Id == model.Id).Select(m => m.Properties.Where(x=>x.Attribute.Status == true)).SelectMany(x => x);
                    foreach (var attr in attributes.Select(a=>a.Attribute).Distinct())
                    {
                        filter = new Filters();
                        filter.Attribute = attr;
                        filter.Max_Value = attr.Values.Select(v=>v.Double_Value).Max();
                        filter.Min_Value = attr.Values.Select(v => v.Double_Value).Min();
                        filter.String_Values = attr.Values.Select(v => v.String_Value).ToList();
                        filters.Add(filter);

                    }


                ViewBag.Filters = filters;
            ViewBag.CatID = cat_id;

            return View(model);
        }
        public PartialViewResult CatalogueOfProducts(int cat_id)
        {
            Category model = null;
            using (var context = new CodeContext())
            {
                model = context.Categories.Find(cat_id);
                context.Attributes.ToList();
                int parent_id = 0;
                if (model.Parent != null)
                    parent_id = model.Parent.Id;
                model.Attributes = context.Categories.Where(c => c.Id == model.Id || c.Id == parent_id)
                                    .Select(c => c.Attributes).ToList().SelectMany(x => x).Distinct().ToList();
            }
            var products_models = model.Products.Select(m => m.Models);
            string models_id = "";
            foreach (var models in products_models)
            {
                foreach (var mod in models)
                {
                    models_id += mod.Id.ToString() + ";";
                }
            }
            ViewData["Models"] = models_id;
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult CatalogueOfProducts(List<double> Max_Value, List<double> Min_Value, List<string> String_Values, List<int> attr_id, int cat_id)
        {

            Category model = null;
            var context = new CodeContext();

            context.Attributes.ToList();
            context.Categories.ToList();
            context.Products.ToList();
            context.Models.ToList();
            context.AttributesValues.ToList();
            model = context.Categories.Find(cat_id);
            int parent_id = 0;
            if (model.Parent != null)
                parent_id = model.Parent.Id;
            model.Attributes = context.Categories.Where(c => c.Id == model.Id || c.Id == parent_id)
                                .Select(c => c.Attributes).ToList().SelectMany(x => x).Distinct().ToList();
            List<Product> products = context.Products.ToList();
            List<Model> models_prods = new List<Model>();
            for (int i = 0; i < attr_id.Count; i++)
            {
                if(context.Attributes.Find(attr_id[i]).Type == Types.Double)
                { }
            }
            Filters attribute;
            int double_index = 0;
            foreach (var product in products)
            {
                double_index = 0;
                for(int i = 0; i < attr_id.Count; i++)
                {
                    int attribute_id = attr_id[i];
                    attribute = new Filters();
                    attribute.Attribute = context.Attributes.Find(attribute_id);

                    if (context.Attributes.Find(attribute_id).Type == Types.Double)
                    {
                        attribute.Max_Value = Max_Value[double_index];
                        attribute.Min_Value = Min_Value[double_index];
                        product.Models = product.Models
                            .Where(m => m.Properties.Where(p => p.Attribute == attribute.Attribute).FirstOrDefault().Double_Value <= attribute.Max_Value
                            && m.Properties.Where(p => p.Attribute == attribute.Attribute).FirstOrDefault().Double_Value >= attribute.Min_Value).ToList();
                        double_index++;
                    }
                    if (context.Attributes.Find(attribute_id).Type == Types.String)
                    {
                        if (String_Values != null)
                        {
                            List<string> str_values = String_Values.Where(str => str.Contains(attribute_id.ToString())).ToList();
                            for (int j = 0; j < str_values.Count; j++)
                            {
                                str_values[j] = str_values[j].Split('_')[0];
                                str_values[j] = str_values[j].Remove(str_values[j].Length - 1);
                            }
                            try
                            {
                                if (product.Models.Select(m => m.Properties.Where(p => p.Attribute == attribute.Attribute)).Count() != 0)
                                    product.Models = product.Models.Where(m => str_values.Contains(m.Properties.Where(p => p.Attribute == attribute.Attribute)
                                .FirstOrDefault().String_Value)).ToList();
                                else
                                    product.Models = new List<Model>();
                            }
                            catch (Exception e) { }
                        }

                    }
                }
            }
            model.Products = products.Where(p => p.Models.Count != 0).ToList();

            return PartialView(model);

        }

        public ActionResult product(int product_id, List<int> selected_models)
        {
            var context = new CodeContext();
            context.Attributes.ToList();
            context.AttributesValues.ToList();

            context.Categories.ToList();
            Product model = context.Products.Find(product_id);
            int parent_id = 0;
            if (model.Category.Parent != null)
                parent_id = model.Category.Parent.Id;

            model.Category.Attributes = context.Categories.Where(c => c.Id == model.Id || c.Id == parent_id)
                                .Select(c => c.Attributes).ToList().SelectMany(x => x).Distinct().ToList();

            Dictionary<string, string> properties = new Dictionary<string, string>();
            string value;
            foreach (var attributes in context.Models.Where(m => m.Product.Id == model.Id).Select(m => m.Properties))
            {
                foreach (var attr in attributes)
                {
                    if (attr.Attribute.Type == Types.Double)
                    {
                        value = " " + attributes.Where(a => a == attr).Select(v => v.Double_Value).Min().ToString()
                            + " : " + attributes.Where(a => a == attr).Select(v => v.Double_Value).Max().ToString()
                            + attr.Attribute.Unit;
                        properties.Add(attr.Attribute.Name, value);
                    }
                }
            }
                        
            List<Model> selected = context.Models.Where(m => selected_models.Contains(m.Id)).ToList();
            ViewBag.Attr = properties;
            ViewBag.SelectedModels = selected;

            return View(model);
        }




    }
}