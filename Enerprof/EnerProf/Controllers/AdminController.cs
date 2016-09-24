using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnerProf.Models;
using EnerProf.DataBaseClasses;

namespace EnerProf.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index(int page = 1)
        {

            int page_size = 15;
            List<Product> model = new List<Product>();
            var context = new CodeContext();
            context.Categories.ToList();
            context.Attributes.ToList();
            context.AttributesValues.ToList();
            context.Models.ToList();

            var prods = context.Products.ToList().Skip((page - 1) * page_size).Take(page_size);
            model = new List<Product>();
            foreach (var p in prods)
            model.Add(p);

            ViewBag.PagingInfo = new PagesInfo { CurrentPage = page, ItemsPerPage = page_size, TotalItems = model.Count };
            return View(model);

        }
        public ActionResult product(int? id)
        {
            Product model = new Product();
            var context = new CodeContext();
            context.Categories.ToList();
            context.Attributes.ToList();
            context.AttributesValues.ToList();
            context.Models.ToList();

            model = context.Products.Find(id);
            if (model != null)
                ViewBag.Cats = context.Categories.Where(cat => cat.Id != model.Category.Id).ToList();
            else
            {
                model = new Product();
                ViewBag.Cats = context.Categories.ToList();
            }
            ViewBag.Products = context.Products.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult product(Product model, int Category, int[] same_products, HttpPostedFileBase Image)
        {
            var context = new CodeContext();

            Product prod = context.Products.Find(model.Id);
            if (prod == null)
                prod = new Product();
            model.Category = context.Categories.Find(Category);
            if (Image != null)
            {
                Image.SaveAs(Server.MapPath("/images/" + Image.FileName));
                string value = "/images/" + Image.FileName;
                prod.Img = value;
            }

            prod.Name = model.Name;
            prod.Category = model.Category;
            prod.Description = model.Description;

            prod.SameProducts = new List<Product>();
            if(same_products != null)
                foreach (var item in same_products)
                    prod.SameProducts.Add(context.Products.Find(item));

            if (context.Products.Find(model.Id) == null)
                context.Products.Add(prod);

            context.SaveChanges();
            return product(model.Id);
        }

        public PartialViewResult models(int? product_id)
        {
            var context = new CodeContext();
            context.Products.ToList();
            IEnumerable<Model> models = context.Models.ToList();
            List<Product> products = models.Select(m => m.Product).ToList();
            if (product_id != null)
                models = models.Where(m => m.Product.Id == product_id);
            ViewBag.Products = products;

            return PartialView(models);
        }

        public ActionResult model(int? model_id, int product_id)
        {
            var context = new CodeContext();
            context.Products.ToArray();
            context.Attributes.ToList();
            context.AttributesValues.ToList();
            context.Categories.ToList();
            Model model;
            if (model_id != null)
                model = context.Models.Find(model_id);
            else
                model = new Model();
            model.Product = context.Products.Find(product_id);
            if(model.Properties == null)
            {
                List<Category> cat_and_parent = new List<Category>();
                cat_and_parent.Add(model.Product.Category);
                cat_and_parent.Add(model.Product.Category.Parent);
                int parent_id = 0;
                if (model.Product.Category.Parent != null)
                    parent_id = model.Product.Category.Parent.Id;
                var list_attr = context.Categories.Where(c => c.Id == model.Product.Category.Id || c.Id == parent_id)
                    .Select(c => c.Attributes).ToList();
                List<AttributeModel> attributes = list_attr.SelectMany(x => x).Distinct().ToList();
                List<AttributesValues> model_props = new List<AttributesValues>();
                AttributesValues attr_value;
                foreach (var item in attributes)
                {
                        attr_value = new AttributesValues();
                        attr_value.Attribute = item;
                    attr_value.Double_Value = null;
                    attr_value.String_Value = null;
                    model_props.Add(attr_value);
                }
                model.Properties = model_props;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult model(Model m, object[] value, int Product)
        {
            var context = new CodeContext();
            context.Products.ToList();
            context.Attributes.ToList();
            context.AttributesValues.ToList();
            context.Categories.ToList();
            Model model = context.Models.Find(m.Id);
            if (model == null)
            {
                model = new Model();
                model.Product = context.Products.Find(Product);

                int parent_id = 0;
                if (model.Product.Category.Parent != null)
                    parent_id = model.Product.Category.Parent.Id;

                var list_attr = context.Categories.Where(c => c.Id == model.Product.Category.Id || c.Id == parent_id)
                                    .Select(c => c.Attributes).ToList();
                List<AttributeModel> attributes = list_attr.SelectMany(x => x).Distinct().ToList();
                List<AttributesValues> model_props = new List<AttributesValues>();
                AttributesValues attr_value;
                foreach (var item in attributes)
                {
                    attr_value = new AttributesValues();
                    attr_value.Attribute = item;
                    attr_value.Double_Value = null;
                    attr_value.String_Value = null;
                    model_props.Add(attr_value);
                }
                model.Properties = model_props;
            }
            model.Name = m.Name;
            for (int i = 0; i < value.Length; i++)
            {
                if (model.Properties.ToList()[i].Attribute.Type == Types.Double)
                {
                    if (value[i] != "")
                        model.Properties.ToList()[i].Double_Value = Convert.ToDouble(value[i]);
                    else
                        model.Properties.ToList()[i].Double_Value = null;
                }
                else if (model.Properties.ToList()[i].Attribute.Type == Types.String)
                    model.Properties.ToList()[i].String_Value = (string)value[i];
            }
            if (context.Models.Find(m.Id) == null)
                context.Models.Add(model);
            context.SaveChanges();
            return this.model(model.Id, model.Product.Id);
        }

        public ActionResult categories()
        {
            var context = new CodeContext();
            List<Category> model = context.Categories.ToList();

            return View(model);
        }

        public ActionResult category(int? cat_id)
        {
            CodeContext context = new CodeContext();
            context.Categories.ToList();
            context.Attributes.ToList();
            context.AttributesValues.ToList();
            context.Products.ToList();
            
            Category model = context.Categories.Find(cat_id);
            if (model == null)
            {
                model = new Category();
                ViewBag.Cats = context.Categories.Where(cat => cat.Id != cat_id).ToList();
            }
            else
                ViewBag.Cats = context.Categories.ToList();

            ViewBag.Attributes = context.Attributes.ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult category(Category cat, int[] attributes, int Category, HttpPostedFileBase Image)
        {
            var context = new CodeContext();
            context.Attributes.ToArray();
            context.Categories.ToList();
            context.Products.ToList();
            Category model = context.Categories.Find(cat.Id);
            if (model == null)
                model = new Category();
            if(Image != null)
            {
                Image.SaveAs(Server.MapPath("/images/" + Image.FileName));
                string value = "/images/" + Image.FileName;
                cat.Img = value;
                model.Img = cat.Img;

            }
            if (attributes != null)
                cat.Attributes = context.Attributes.Where(a => attributes.Contains(a.id)).ToList();
            if (Category != 0)
                cat.Parent = context.Categories.Find(Category);

            if (model.Attributes != null)
                model.Attributes.Clear();
            else
                model.Attributes = new List<AttributeModel>();
            if(cat.Attributes != null)
                foreach (var attr in cat.Attributes)
                    model.Attributes.Add(attr);

            model.Description = cat.Description;
            model.IsOnStartPage = cat.IsOnStartPage;
            model.Name = cat.Name;
            model.Parent = cat.Parent;

            if (context.Categories.Find(cat.Id) == null)
                context.Categories.Add(model);
            

            context.SaveChanges();

            return category(cat.Id);
        }

        public ActionResult attributes()
        {
            var context = new CodeContext();
            List<AttributeModel> model = context.Attributes.ToList();

            return View(model);
        }

        public ActionResult attribute(int? attr_id)
        {
            var context = new CodeContext();
            context.Attributes.ToList();
            context.Categories.ToArray();
            context.Units.ToList();
            AttributeModel model = new AttributeModel();
            if (attr_id != null && attr_id != 0)
                model = context.Attributes.Find(attr_id);

            if (model.Unit != null)
                ViewBag.Units = context.Units.Where(u => u.id != model.Unit.id).ToList();
            else
                ViewBag.Units = context.Units.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult attribute(AttributeModel attr, string new_unit, int? Unit)
        {
            var context = new CodeContext();
            context.Attributes.ToList();
            context.Units.ToList();
            context.Categories.ToList();
            AttributeModel model = context.Attributes.Find(attr.id);
            if (model == null)
                model = new AttributeModel();

            if (new_unit != null && new_unit != "" && context.Units.Where(u => u.Name == new_unit).Count() == 0)
            {
                Unit unit = new Unit() { Name = new_unit };
                context.Units.Add(unit);
                attr.Unit = unit;
            }
            else
                attr.Unit = context.Units.Find(Unit);
            model.Name = attr.Name;
            model.Status = attr.Status;
            model.Type = attr.Type;
            model.Unit = attr.Unit;

            if (context.Attributes.Find(attr.id) == null)
                context.Attributes.Add(model);

            context.SaveChanges();
            return attribute(attr.id);
        }

        public ActionResult industries()
        {
            var context = new CodeContext();
            var model = context.Industries.ToList();
            return View(model);
        }

        public ActionResult industry(int? id)
        {
            Industries model = new Industries();
            var context = new CodeContext();
            if(context.Industries.Find(id) != null)
                model = context.Industries.Find(id);

            ViewBag.Products = context.Products.ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult industry(Industries indust, int[] same_products, HttpPostedFileBase Image)
        {
            var context = new CodeContext();
            Industries model = new Industries();
            if (context.Industries.Find(indust.Id) != null)
                model = context.Industries.Find(indust.Id);
            if (Image != null)
            {
                Image.SaveAs(Server.MapPath("/images/" + Image.FileName));
                string value = "/images/" + Image.FileName;
                model.Img = value;
            }

            model.Description = indust.Description;
            model.Name = indust.Name;
            if (same_products != null)
                foreach (var item in same_products)
                    model.Products.Add(context.Products.Find(item));

            if (context.Industries.Find(indust.Id) == null)
                context.Industries.Add(model);

            context.SaveChanges();

            return industry(indust.Id);

        }


        //public PartialViewResult Models_Attributes(int product_id, int? model_id)
        //{
        //    var context = new CodeContext();
        //    List<AttributeModel> attributes = context.Attributes.Where(a=>a.Category.Id == context.Products.Find(product_id).Category.Id).ToList();
        //    List<AttributesValues> model = new List<AttributesValues>();
        //    AttributesValues attr_value;
        //    foreach (var item in attributes)
        //    {
        //        if(model_id != null && context.AttributesValues.Where(a=>a.Attribute == item && a.Model.Id == model_id).Count() != 0)
        //            attr_value = context.AttributesValues.Where(a => a.Attribute == item && a.Model.Id == model_id).First();
        //        else
        //        {
        //            attr_value = new AttributesValues();
        //            attr_value.Attribute = item;
        //        }
        //        model.Add(attr_value);
        //    }
        //    return PartialView(model);
        //}


        //----------Удаление--------------------------

        public ActionResult DeleteCategory(int cat_id)
        {
            var context = new CodeContext();
            context.Categories.ToArray();
            context.Categories.Remove(context.Categories.Find(cat_id));
            context.SaveChanges();

            return RedirectToAction("categories");
        }
        public ActionResult DeleteAttr(int attr_id)
        {
            var context = new CodeContext();
            context.Categories.ToArray();
            context.Attributes.ToArray();
            List<Category> cats = context.Attributes.Find(attr_id).Categories.ToList();
            foreach (var item in cats)
                item.Attributes.Remove(context.Attributes.Find(attr_id));
            context.AttributesValues.RemoveRange(context.AttributesValues.Where(a => a.Attribute.id == attr_id));

            context.Attributes.Remove(context.Attributes.Find(attr_id));
            context.SaveChanges();

            return RedirectToAction("attributes");
        }
        public ActionResult DeleteProduct(int id)
        {
            var context = new CodeContext();
            context.Products.ToArray();
            context.Products.Remove(context.Products.Find(id));
            context.SaveChanges();

            return RedirectToAction("products");
        }
        public ActionResult DeleteModel(int model_id, int product_id)
        {
            var context = new CodeContext();
            context.Products.ToArray();
            context.Products.Find(product_id).Models.Remove(context.Models.Find(model_id));
            context.SaveChanges();
            context.Models.ToArray();
            context.Models.Remove(context.Models.Find(model_id));
            context.AttributesValues.RemoveRange(context.AttributesValues.Where(a => a.Model.Id == model_id));
            context.SaveChanges();

            return RedirectToAction("product", new { id = product_id });
        }

        //---------КОНЕЦ------Удаление----------------



        [HttpPost]
        public void SaveNewImage(int? id)
        {

            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Files в проекте
                    CodeContext contex = new CodeContext();

                    if (id != null)
                    {
                        Product prod = contex.Products.Find(id);
                        string name = prod.Name;
                        upload.SaveAs(Server.MapPath("/images/" + fileName));
                        string value = prod.Images + "/images/" + fileName + ";";
                        prod.Images = value;
                        contex.SaveChanges();
                    }


                }

            }
        }
        public void Delete(string link, int id)
        {
            var context = new CodeContext();
            Product prod = context.Products.Find(id);
            string images = prod.Files;
            prod.Images = images.Remove(images.IndexOf(link), link.Length + 1);
            context.SaveChanges();
            System.IO.File.Delete(Server.MapPath(link));
        }
        [HttpPost]
        public void SaveNewFile(int? id)
        {

            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Files в проекте
                    CodeContext contex = new CodeContext();

                    if (id != null)
                    {
                        Product prod = contex.Products.Find(id);
                        string name = prod.Name;
                        upload.SaveAs(Server.MapPath("/files/" + fileName));
                        string value = prod.Files + "/files/" + fileName + ";";
                        prod.Files = value;
                        contex.SaveChanges();
                    }


                }

            }
        }
        public void Delete_File(string link, int id)
        {
            var context = new CodeContext();
            Product prod = context.Products.Find(id);
            string images = prod.Files;
            prod.Images = images.Remove(images.IndexOf(link), link.Length + 1);
            context.SaveChanges();
            System.IO.File.Delete(Server.MapPath(link));
        }
        public void DeleteComment(int id)
        {
            var context = new CodeContext();
            context.Comments.Remove(context.Comments.Find(id));
            context.SaveChanges();
        }
    }
}