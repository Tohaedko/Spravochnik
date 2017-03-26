using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Net;
using DB;
using Spravochnik_Api.DictionaryServiceReference;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using NLog;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using RestSharp.Extensions;
using Spravochnik_Api.DTO;

namespace Spravochnik_Api
{
    public class UpdateDB : IUpdateDB
    {
        private RestClient client;
        private DBContext db;
        private Logger logger;

        private UpdateDB()
        {
            client = new RestClient(ConfigurationManager.AppSettings["api_address"]);
            client.Authenticator = new NtlmAuthenticator(ConfigurationManager.AppSettings["api_login"],
                ConfigurationManager.AppSettings["api_password"]);

            logger = LogManager.GetCurrentClassLogger();
        }

        public void Update()
        {
            logger.Trace("Start update DB");
            try
            {
                using (db = new DBContext())
                {
                    var lczList = db.Localization.ToList();
                    var contentLocation = new List<LocationDto>();
                    foreach (var item in lczList)
                    {
                        logger.Trace($"Try to get locations for lang {item.Code}");
                        //send request, get response
                        contentLocation =
                            SendRequestGetResponseContent<LocationDto>(
                                ConfigurationManager.AppSettings["request_Locations"], item.Code, true);
                        logger.Trace($"Try to save locations to DB for lang {item.Code}");
                        SaveToDbLocations(db, item, contentLocation);
                    }

                    lczList = db.Localization.Where(lcz => lcz.Code == "uk").ToList();
                    foreach (var item in lczList)
                    {
                        logger.Trace($"Try to get medicalServiceGroups for lang {item.Code}");
                        //will be in upper foreach (after localization parameter add in SynevoApi)
                        var contentServiceGroups =
                            SendRequestGetResponseContent<MedicalServiceGroupDto>(
                                ConfigurationManager.AppSettings["request_ServiceGroups"], item.Code, false);
                        logger.Trace($"Try to save medicalServiceGroups to DB for lang {item.Code}");
                        SaveToDbServiceGroups(db, item, contentServiceGroups);
                    }

                    lczList = db.Localization.ToList();
                    var contentService = new List<ServiceDto>();
                    foreach (var item in lczList)
                    {
                        logger.Trace($"Try to get services for lang {item.Code}");
                        //send request, get response
                        contentService =
                            SendRequestGetResponseContent<ServiceDto>(
                                ConfigurationManager.AppSettings["request_Services"], item.Code, true);
                        logger.Trace($"Try to save services to DB for lang {item.Code}");
                        SaveToDbServices(db, item, contentService);
                    }

                    var locationsList = db.Location.ToList();
                    var contentPriceList = new List<PriceListDto>();
                    foreach (var item in locationsList)
                    {
                        logger.Trace($"Try to get priceLists for location {item.Code}");
                        //send request, get response
                        contentPriceList =
                            SendRequestGetResponseContent<PriceListDto>(
                                ConfigurationManager.AppSettings["request_PriceList"], item.Code, true);
                        logger.Trace($"Try to save priceLists to DB for locationCode {item.Code}");
                        SaveToDbPriceLists(db, item, contentPriceList);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error($"Exception while processing: {e.Message}\n{e.InnerException}");
                throw;
            }
            finally
            {
                logger.Trace("Start update DB");
            }
        }

        private List<TMyType> SendRequestGetResponseContent<TMyType>(string requestPath, string prefix, bool usePrefix)
        {
            var reqPath = usePrefix ? requestPath + prefix : requestPath;
            var request = new RestRequest(reqPath, Method.GET);
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = JsonConvert.DeserializeObject<List<TMyType>>(response.Content);
                if (content.Count > 0)
                {
                    return content;
                }
            }
            return null;
        }

        private void SaveToDbLocations(DBContext context, Localization lcz, List<LocationDto> content)
        {
            //attach current localize
            context.Localization.Attach(lcz);
            foreach (var item in content)
            {
                var location = context.Location.FirstOrDefault(l => l.Code == item.Id.ToString());

                //create object locationXlocalization
                var locationXlcz = new LocationXLocalization
                {
                    Localization = lcz,
                    LocationName = item.Name
                };

                if (location == null)
                {
                    //create location object
                    location = new Location { Code = item.Id.ToString() };

                    //add to db and save
                    context.Location.Add(location);
                    location.LocationXLocalizations.Add(locationXlcz);
                    context.SaveChanges();
                }
                else
                {
                    var lxlcz = context.LocationXLocalization.FirstOrDefault(
                        lxl => lxl.Location.Code == item.Id.ToString()
                               && lxl.Localization.Id == lcz.Id);
                    if (lxlcz != null)
                    {
                        lxlcz.LocationName = item.Name;
                    }
                    else
                    {
                        location.LocationXLocalizations.Add(locationXlcz);
                    }
                    context.SaveChanges();
                }
            }
        }

        private void SaveToDbServiceGroups(DBContext context, Localization lcz, List<MedicalServiceGroupDto> content)
        {
            //attach current localize
            context.Localization.Attach(lcz);

            foreach (var item in content)
            {
                var serviceGroup = context.MedicalServiceGroup.FirstOrDefault(sg => sg.Code == item.Id.ToString());

                //create object serviceGroupXlocalization
                var serviceGroupXlcz = new MedicalServiceGroupsXLocalization()
                {
                    Localization = lcz,
                    MedicalServiceGroupsName = item.Name
                };

                if (serviceGroup == null)
                {
                    //create location object
                    serviceGroup = new MedicalServiceGroups { Code = item.Id.ToString(), SiteCode = item.Code };

                    //add to db and save
                    context.MedicalServiceGroup.Add(serviceGroup);
                    serviceGroup.MedicalServiceGroupsXLocalization.Add(serviceGroupXlcz);
                    context.SaveChanges();
                }
                else
                {
                    var sgxlcz = context.MedicalServiceGroupsXLocalization.FirstOrDefault(
                        sgxl => sgxl.MedicalServiceGroups.Code == item.Id.ToString()
                               && sgxl.Localization.Id == lcz.Id);
                    if (sgxlcz != null)
                    {
                        sgxlcz.MedicalServiceGroupsName = item.Name;
                    }
                    else
                    {
                        serviceGroup.MedicalServiceGroupsXLocalization.Add(serviceGroupXlcz);
                    }
                    context.SaveChanges();
                }
            }

            //add parent serviceGroup to each serviceGroup
            foreach (var item in content)
            {
                var serviceGroup = context.MedicalServiceGroup.Single(sg => sg.Code == item.Id.ToString());
                if (item.ParentGroupId > 0)
                {
                    serviceGroup.ParentGroupId =
                        context.MedicalServiceGroup.Single(sg => sg.Code == item.ParentGroupId.ToString());
                }
            }
            context.SaveChanges();
        }

        private void SaveToDbServices(DBContext context, Localization lcz, List<ServiceDto> content)
        {
            //attach current localize
            context.Localization.Attach(lcz);

            foreach (var item in content)
            {
                var service = context.Service.FirstOrDefault(s => s.Code == item.Id.ToString());

                //create object serviceGroupXlocalization
                var serviceXlcz = new ServiceXLocalization()
                {
                    Localization = lcz,
                    ServiceName = item.Name,
                    Description = item.Description,
                    Keywords = item.Keywords,
                    Remark = item.Remark
                };

                if (service == null)
                {
                    //create location object
                    service = new Service()
                    {
                        Code = item.Id.ToString(),
                        SiteCode = item.Code,
                        IsDiscountAllowed = item.IsDiscountAllowed
                    };

                    //add to db and save
                    context.Service.Add(service);
                    service.ServiceXLocalization.Add(serviceXlcz);

                    foreach (var msg in item.MedicalServiceGroups)
                    {
                        service.ServiceXMedicalServiceGroups.Add(new ServiceXMedicalServiceGroups()
                        {
                            MedicalServiceGroups = context.MedicalServiceGroup.Single(sg => sg.Code == msg.ToString()),
                            Service = service
                        });
                    }

                    context.SaveChanges();
                }
                else
                {
                    var sxlcz = context.ServiceXLocalization.FirstOrDefault(
                        sxl => sxl.Services.Code == item.Id.ToString()
                               && sxl.Localization.Id == lcz.Id);
                    if (sxlcz != null)
                    {
                        sxlcz.ServiceName = item.Name;
                        sxlcz.Description = item.Description;
                        sxlcz.Keywords = item.Keywords;
                        sxlcz.Remark = item.Remark;
                    }
                    else
                    {
                        service.ServiceXLocalization.Add(serviceXlcz);
                    }
                    context.SaveChanges();
                }
            }
            //add parent serviceGroup to each serviceGroup
            foreach (var item in content.Where(c => c.Components.Count() > 0))
            {
                var service = context.Service.Single(s => s.Code == item.Id.ToString());
                context.ServiceXService.RemoveRange(service.ServiceXService);
                context.SaveChanges();

                foreach (var comp in item.Components)
                {
                    service.ServiceXService.Add(new ServiceXService()
                    {
                        //NEED CHANGE TO Single 
                        ComponentServiceId = context.Service.SingleOrDefault(s => s.Code == comp.ToString()),
                        ParentServiceId = service
                    });
                }
                context.SaveChanges();
            }

        }

        private void SaveToDbPriceLists(DBContext context, Location location, List<PriceListDto> content)
        {
            //attach current location
            context.Location.Attach(location);

            foreach (var item in content)
            {
                var priceList = context.PriceList.FirstOrDefault(p => p.Services.Code == item.ServiceId.ToString()
                && p.Location.Code == item.LocationId.ToString());

                if (priceList == null)
                {
                    //create and add priceList object
                    context.PriceList.Add(new PriceList()
                    {
                        Location = location,
                        Price = item.Price,
                        Services = context.Service.Single(s => s.Code == item.ServiceId.ToString()),
                        Workingdays = item.Workingdays
                    });
                    //save
                    context.SaveChanges();
                }
                else
                {
                    priceList.Price = item.Price;
                    priceList.Workingdays = item.Workingdays;
                    context.SaveChanges();
                }
            }
        }
    }
}