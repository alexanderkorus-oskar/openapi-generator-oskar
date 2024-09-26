/**
 * OpenAPI Petstore
 * This spec is mainly for testing Petstore server and contains fake endpoints, models. Please do not use this for any other purpose. Special characters: \" \\
 *
 * The version of the OpenAPI document: 1.0.0
 * 
 *
 * NOTE: This class is auto generated by OpenAPI-Generator 7.8.0-SNAPSHOT.
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */

/*
 * Client.h
 *
 * 
 */

#ifndef Client_H_
#define Client_H_



#include <string>
#include <memory>
#include <vector>
#include <boost/property_tree/ptree.hpp>
#include "helpers.h"

namespace org {
namespace openapitools {
namespace server {
namespace model {

/// <summary>
/// 
/// </summary>
class  Client 
{
public:
    Client() = default;
    explicit Client(boost::property_tree::ptree const& pt);
    virtual ~Client() = default;

    Client(const Client& other) = default; // copy constructor
    Client(Client&& other) noexcept = default; // move constructor

    Client& operator=(const Client& other) = default; // copy assignment
    Client& operator=(Client&& other) noexcept = default; // move assignment

    std::string toJsonString(bool prettyJson = false) const;
    void fromJsonString(std::string const& jsonString);
    boost::property_tree::ptree toPropertyTree() const;
    void fromPropertyTree(boost::property_tree::ptree const& pt);


    /////////////////////////////////////////////
    /// Client members

    /// <summary>
    /// 
    /// </summary>
    std::string getClient() const;
    void setClient(std::string value);

protected:
    std::string m_Client = "";
};

std::vector<Client> createClientVectorFromJsonString(const std::string& json);

template<>
inline boost::property_tree::ptree toPt<Client>(const Client& val) {
    return val.toPropertyTree();
}

template<>
inline Client fromPt<Client>(const boost::property_tree::ptree& pt) {
    Client ret;
    ret.fromPropertyTree(pt);
    return ret;
}

}
}
}
}

#endif /* Client_H_ */
