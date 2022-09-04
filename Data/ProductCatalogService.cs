using Mvc_Apteka;
using Mvc_Apteka.Entities;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfGrid.Data
{
    public class ProductCatalogService
    {

        public List<ProductCatalog> GetChilren(List<ProductCatalog> nodes, ProductCatalog parent)
        {
            return nodes.Where(p => p.Parent == parent).ToList();
        }
        public void AddNode(AppDbContext context, List<ProductCatalog> nodes, ProductCatalog pnode, int? parentId)
        {
            pnode.ParentID = parentId;
            context.ProductCatalogs.Add(pnode);
            context.SaveChanges();

            foreach(var node in GetChilren(nodes, pnode))
            {
                AddNode(context, nodes, node, pnode.ID);
                Console.WriteLine(  $"ParentID={pnode.ParentID}; ParentName={pnode.ProductCatalogName}" );
            }
        }
        public ProductCatalog GetRoot(IEnumerable<ProductCatalog> nodes)
        {
            var roots = nodes.Where(p => p.Parent == null);
            if (roots.Count() != 1)
            {
                throw new Exception($"Корень должен быть один а не {roots.Count()}");
            }
            return roots.First();
        }
        public void AddHier(AppDbContext context, List<ProductCatalog> nodes)
        {
            var roots = nodes.Where(p => p.Parent==null);
            if (roots.Count() != 1)
            {
                throw new Exception($"Корень должен быть один а не {roots.Count()}");
            }
            var root = roots.First();
            AddNode(context, nodes, roots.First(), null);
        }
        
        public void Print(TextWriter output, AppDbContext context, ProductCatalog catalog)
        {
            string message = GetQualificationString(catalog);
            output.WriteLine(message);
            foreach (var child in GetProductSubCatalogs(context, catalog)) 
            {
                Print(output, context, child);
            }
        }

        public List<string> GetQualification(AppDbContext context, ProductCatalog catalog)
        {
            List<string> qnames = (catalog.ParentID != null) ?
                GetQualification(context, context.ProductCatalogs.Find(catalog.ParentID)) :
                new List<string>();
            qnames.Add(catalog.ProductCatalogName);
            return qnames;
        }

        public List<ProductCatalog> GetProductSubCatalogs(AppDbContext context, ProductCatalog catalog)
            => context.ProductCatalogs.Where(c => c.Parent == catalog).ToList();

        public string GetQualificationString(AppDbContext context, ProductCatalog catalog)
        {
            string path = (catalog.ParentID != null) ?
                GetQualificationString(context, context.ProductCatalogs.Find(catalog.ParentID)) :
                "";
            path += (catalog.ProductCatalogName) + "/";
            return path;
        }
        public string GetQualificationString( ProductCatalog catalog)
        {
            string path = (catalog.Parent != null) ?
                GetQualificationString(catalog.Parent) :
                "";
            path += (catalog.ProductCatalogName) + "/";
            return path;
        }
    }
}
