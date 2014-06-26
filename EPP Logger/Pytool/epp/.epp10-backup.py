__author__ = "Alex Skinner <alex.skinner@neustar.biz>"
__license__ = "GPLv3"

import socket, ssl, struct, string, random, time, sys
import xml.etree.cElementTree as xml

#Each class instance should represent one connection to one server and handle one command at a time.
#Class variables are isolated from other instances, but not itself.
#That is, you should NOT thread a single object!

class EPP10():
    def __init__(self):
        self.counter = 0
        self.top = """<?xml version="1.0" encoding="UTF-8"?>"""
        self.rnd = self.gen_random(3)


    def connect(self, server, port, key=None, cert=None, cafile=None, usetls=1):
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        if usetls:
            self.eppsock = ssl.wrap_socket(s, keyfile=key, certfile=cert, ca_certs=cafile, cert_reqs=ssl.CERT_REQUIRED)
        else:
            self.eppsock = s
        self.eppsock.connect((server, port))
        return self.receive()



    def receive(self):
        response = ''
        readdata = 4
        header = self.eppsock.recv(4)
        numbytes = socket.ntohl(struct.unpack('=L',header)[0])
        while readdata < numbytes:
            try:
                resp = self.eppsock.recv(min(numbytes-readdata,16384))
                readdata += len(resp)
                response += str(resp, 'utf-8')
            except:
                pass
           
        return response


    def send(self, xmltosend):
        xmltosend = xmltosend.encode()
        header = struct.pack('=L',socket.htonl(len(xmltosend)+4))
        self.eppsock.send(header)
        return self.eppsock.send(xmltosend)



    def send_receive(self, xmltosend):
            self.send(xmltosend)
            return(self.receive())




    def disconnect(self):
        self.eppsock.shutdown(2)
        self.eppsock.close()



    def gen_head(self):
        self.epp=xml.Element('epp', {'xmlns':"urn:ietf:xml:ns:epp-1.0"})
        self.elcommand = xml.SubElement(self.epp, 'command')
        self.counter += 1
 


    def gen_tail(self):
        fulldate = time.strftime("%Y%m%d-%H%M%S")
        xml.SubElement(self.elcommand, 'clTRID').text = "%s-%s-%s" % (fulldate, self.rnd, self.counter)





    def gen_random(self, len):
        alphanum = string.ascii_letters + string.digits
        retval = [random.choice(alphanum) for i in range(len)]
        return(''.join(retval))





    def gen_unspec(self, unspec=None, nvtup=None, add_ds_record=None, remove_ds_record=None, ds_record=None):
        if any((unspec, nvtup, add_ds_record, remove_ds_record, ds_record)):
            elextension = xml.SubElement(self.elcommand, 'extension')
            if ds_record:
                ds_record = self.gen_list(ds_record)
                elsecdns = xml.SubElement(elextension, 'secDNS:create', {'xmlns:secDNS':"urn:ietf:params:xml:ns:secDNS-1.0"})
                for ea in ds_record:
                    try:
                       ea = ea.split(',')
                    except:
                       pass
                    if len(ea) != 4:
                        return 'Should be 4 elements in list (keytag, alg, digesttype, digest)'
                    eldsdata = xml.SubElement(elsecdns, 'dsData')
                    xml.SubElement(eldsdata, 'secDNS:keyTag').text = str(ea[0])
                    xml.SubElement(eldsdata, 'secDNS:alg').text = str(ea[1])
                    xml.SubElement(eldsdata, 'secDNS:digestType').text = str(ea[2])
                    xml.SubElement(eldsdata, 'secDNS:digest').text = str(ea[3])
            if add_ds_record or remove_ds_record:
                elsecdns = xml.SubElement(elextension, 'secDNS:update', {'xmlns':"urn:ietf:params:xml:ns:secDNS-1.0"})
                
                if add_ds_record:
                    add_ds_record = self.gen_list(add_ds_record)
                    eladd = xml.SubElement(elsecdns, 'secDNS:add')
                    for ea in add_ds_record:
                        try:
                            ea = ea.split(',')
                        except:
                            pass
                        if len(ea) != 4:
                            return 'Should be 4 elements in list (keytag, alg, digesttype, digest)'
                        eldsdata = xml.SubElement(eladd, 'secDNS:dsData')
                        xml.SubElement(eldsdata, 'secDNS:keyTag').text = str(ea[0])
                        xml.SubElement(eldsdata, 'secDNS:alg').text = str(ea[1])
                        xml.SubElement(eldsdata, 'secDNS:digestType').text = str(ea[2])
                        xml.SubElement(eldsdata, 'secDNS:digest').text = str(ea[3])
                if remove_ds_record:
                    remove_ds_record = self.gen_list(remove_ds_record)
                    elrem = xml.SubElement(elsecdns, 'secDNS:rem')
                    remove_ds_record = self.gen_list(remove_ds_record)
                    for ea in remove_ds_record:
                        xml.SubElement(elrem, 'secDNS:keyTag').text = str(ea)

            if unspec or nvtup:
                elneulevelext = xml.SubElement(elextension, 'neulevel:extension', {'xmlns':"urn:ietf:params:xml:ns:neulevel-1.0"})
                
                if unspec:
                    unspec = self.gen_list(unspec)
                    xml.SubElement(elneulevelext, 'unspec').text = ' '.join(unspec)
                if nvtup:
                    nvtup = self.gen_list(nvtup)
                    elgeneric = xml.SubElement(elneulevelext, 'genericNVPairs')
                    for tup in nvtup:
                        xml.SubElement(elgeneric, 'NVTuple', {'name':tup[0]}).text = tup[1]




    def gen_list(self, initem):
        try:
            initem = initem.strip().split('|')
        except:
            return initem
        return initem




    def login(self, username, password, new_password=None):
        self.gen_head()
        ellogin = xml.SubElement(self.elcommand, 'login')
        xml.SubElement(ellogin, 'clID').text = username
        xml.SubElement(ellogin, 'pw').text = password
        if new_password:
            xml.SubElement(ellogin, 'newPW').text = new_password
        elopt = xml.SubElement(ellogin, 'options')
        xml.SubElement(elopt, 'version').text = '1.0'
        xml.SubElement(elopt, 'lang').text = 'en-US'
        elsvcs = xml.SubElement(ellogin, 'svcs')
        xml.SubElement(elsvcs, 'objURI').text = 'urn:ietf:params:xml:ns:contact'
        xml.SubElement(elsvcs, 'objURI').text = 'urn:ietf:params:xml:ns:host'
        xml.SubElement(elsvcs, 'objURI').text = 'urn:ietf:params:xml:ns:domain'
        elsvcext = xml.SubElement(elsvcs, 'svcExtension')
        xml.SubElement(elsvcext, 'extURI').text = 'urn:ietf:params:xml:ns:neulevel'
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))




    def hello(self):
        elepp = xml.Element('epp', {'xmlns':"urn:ietf:params:xml:ns:epp-1.0"})
        xml.SubElement(elepp, 'hello')
        return(self.top + xml.tostring(elepp, 'unicode'))


    def poll(self, op=None, msg_id=None):
        self.gen_head()
        if msg_id:
            xml.SubElement(self.elcommand, 'poll', {'op': op, 'msgID': msgID})
        else:
            xml.SubElement(self.elcommand, 'poll', {'op': op})
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))

    def logout(self):
        self.gen_head()
        xml.SubElement(self.elcommand, 'logout')
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))




    def contact_create(self, contact_id=None, name=None, org=None, street=None, city=None, state=None, postal=None, country_code=None, voice=None, fax=None, email=None, pw=None, unspec=None, intl_name=None, intl_org=None, intl_street=None, intl_city=None, intl_state=None, intl_postal=None, intl_country_code=None):

        self.gen_head()
        elcreate = xml.SubElement(self.elcommand, 'create')
        elcontactcreate = xml.SubElement(elcreate, 'contact:create', {'xmlns:contact':"urn:ietf:params:xml:ns:contact-1.0"})
        xml.SubElement(elcontactcreate, 'contact:id').text = contact_id
        elpostinfo = xml.SubElement(elcontactcreate, 'contact:postalInfo', {'type':'int'})
        xml.SubElement(elpostinfo, 'contact:name').text = name
        if org: 
            xml.SubElement(elpostinfo, 'contact:org').text = org
        eladdr = xml.SubElement(elpostinfo, 'contact:addr')
        if street:
            street = self.gen_list(street)
            for streetline in street:
                xml.SubElement(eladdr, 'contact:street').text = streetline
        xml.SubElement(eladdr, 'contact:city').text = city
        if state:
            xml.SubElement(eladdr, 'contact:sp').text = state
        if postal:
            xml.SubElement(eladdr, 'contact:pc').text = str(postal)
        xml.SubElement(eladdr, 'contact:cc').text = country_code

        if any((intl_name, intl_org, intl_street, intl_city, intl_state, intl_postal, intl_country_code)):
            elintpostinfo = xml.SubElement(elcontactcreate, 'contact:postalInfo', {'type':'loc'})
            if intl_name:
                xml.SubElement(elintpostinfo, 'contact:name').text = intl_name
            if intl_org:
                xml.SubElement(elintpostinfo, 'contact:org').text = intl_org
            if any((intl_street, intl_city, intl_state, intl_postal, intl_country_code)):
                elintaddr = xml.SubElement(elintpostinfo, 'contact:addr')
                intl_street = self.gen_list(intl_street)
                for stree in intl_street:
                    xml.SubElement(elintaddr, 'contact:street').text = stree
                xml.SubElement(elintaddr, 'contact:city').text = intl_city
                if intl_state:
                    xml.SubElement(elintaddr, 'contact:sp').text = intl_state
                if intl_postal:
                    xml.SubElement(elintaddr, 'contact:pc').text = intl_postal
                xml.SubElement(elintaddr, 'contact:cc').text = intl_country_code
               
        if 'x' in voice:
            xml.SubElement(elcontactcreate, 'contact:voice', {'x':voice.split('x')[-1]}).text = voice.split('x')[0]
        else:
            xml.SubElement(elcontactcreate, 'contact:voice').text = voice
        if fax:
            if 'x' in fax:
                elfax = xml.SubElement(elcontactcreate, 'contact:fax', {'x':fax.split('x')[-1]}).text = fax.split('x')[0]
            else:
                xml.SubElement(elcontactcreate, 'contact:fax').text = fax
        xml.SubElement(elcontactcreate, 'contact:email').text = email
        elpw = xml.SubElement(elcontactcreate, 'contact:authInfo')
        xml.SubElement(elpw, 'contact:pw').text = pw
        self.gen_unspec(unspec)
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))



    def contact_check(self, contact_id=None):
        self.gen_head()
        elcheck = xml.SubElement(self.elcommand, 'check')
        elcontactcheck = xml.SubElement(elcheck, 'contact:check', {'xmlns:contact':"urn:ietf:params:xml:ns:contact-1.0"})
        elcontact_id = xml.SubElement(elcontactcheck, 'contact:id')
        elcontact_id.text = contact_id
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))



    def contact_info(self, contact_id=None):
        self.gen_head()
        elinfo = xml.SubElement(self.elcommand, 'info')
        elcontactinfo = xml.SubElement(elinfo, 'contact:info', {'xmlns:contact':"urn:ietf:params:xml:ns:contact-1.0"})
        xml.SubElement(elcontactinfo, 'contact:id').text = contact_id
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))



    def contact_transfer(self, contact_id=None, op=None, pw=None, unspec=None):
        self.gen_head()
        eltransfer = xml.SubElement(self.elcommand, 'transfer', {'op':op})
        elcontacttransfer = xml.SubElement(eltransfer, 'contact:transfer', {'xmlns:contact':"urn:ietf:params:xml:ns:contact-1.0"})
        xml.SubElement(elcontacttransfer, 'contact:id').text = contact_id
        if op.lower() == "request":
            elcontactpw = xml.SubElement(elcontacttransfer, 'contact:authInfo')
            xml.SubElement(elcontactpw, 'contact:pw').text = pw
        self.gen_unspec(unspec)
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))



    def contact_update(self, contact_id=None, name=None, org=None, street=None, city=None, state=None, postal=None, country_code=None, voice=None, fax=None, email=None, pw=None, add_status=None, remove_status=None,unspec=None,intl_name=None, intl_org=None, intl_street=None, intl_city=None, intl_state=None, intl_postal=None, intl_country_code=None):
        self.gen_head()
        elupdate = xml.SubElement(self.elcommand, 'update')
        elcontactupdate = xml.SubElement(elupdate, 'contact:update', {'xmlns:contact':"urn:ietf:params:xml:ns:contact-1.0"})
        xml.SubElement(elcontactupdate, 'contact:id').text = contact_id
        if add_status:
            add_status = self.gen_list(add_status)
            elcontactadd = xml.SubElement(elcontactupdate, 'contact:add')
            for status in add_status:
                xml.SubElement(elcontactadd, 'contact:status', {'s':status})
        if remove_status:
            remove_status = self.gen_list(remove_status)
            elcontactrem = xml.SubElement(elcontactupdate, 'contact:rem')
            for status in remove_status:
                xml.SubElement(elcontactrem, 'contact:status', {'s':status})
        if any((name, org, street, city, state, postal, country_code)):
            elcontactchg = xml.SubElement(elcontactupdate, 'contact:chg')
            elcontactpost = xml.SubElement(elcontactchg, 'contact:postalInfo', {'type':'int'})
            if name:
                xml.SubElement(elcontactpost, 'contact:name').text = name
            if org:
                xml.SubElement(elcontactpost, 'contact:org').text = org
            if street or state or city or postal or country_code:
                eladdr = xml.SubElement(elcontactpost, 'contact:addr')
                street = self.gen_list(street)
                for item in street:
                    xml.SubElement(eladdr, 'contact:street').text = item
                xml.SubElement(eladdr, 'contact:city').text = city
                if state:
                    xml.SubElement(eladdr, 'contact:sp').text = state
                if postal:
                    xml.SubElement(eladdr, 'contact:pc').text = postal
                xml.SubElement(eladdr, 'contact:cc').text = country_code
        if any((intl_name, intl_org, intl_street, intl_city, intl_state, intl_postal, intl_country_code)):
            elintpostinfo = xml.SubElement(elcontactchg, 'contact:postalInfo', {'type':'loc'})
            if intl_name:
                xml.SubElement(elintpostinfo, 'contact:name').text = intl_name
            if intl_org:
                xml.SubElement(elintpostinfo, 'contact:org').text = intl_org
            if any((intl_street, intl_city, intl_state, intl_postal, intl_country_code)):
                elintaddr = xml.SubElement(elintpostinfo, 'contact:addr')
                if intl_street:
                    intl_street = self.gen_list(intl_street)
                    for stree in intl_street:
                        xml.SubElement(elintaddr, 'contact:street').text = stree
                xml.SubElement(elintaddr, 'contact:city').text = intl_city
                if intl_state:
                    xml.SubElement(elintaddr, 'contact:sp').text = intl_state
                if intl_postal:
                    xml.SubElement(elintaddr, 'contact:pc').text = intl_postal
                xml.SubElement(elintaddr, 'contact:cc').text = intl_country_code
        if voice:
            if 'x' in voice:
                xml.SubElement(elcontactchg, 'contact:voice', {'x':voice.split('x')[-1]}).text = voice.split('x')[0]
            else:
                xml.SubElement(elcontactchg, 'contact:voice').text = voice
        if fax:
            if 'x' in fax:
                xml.SubElement(elcontactchg, 'contact:fax', {'x':fax.split('x')[-1]}).text = fax.split('x')[0]
            else:
                xml.SubElement(elcontactchg, 'contact:fax').text = fax
        if pw:
            elpw = xml.SubElement(elcontactchg, 'contact:authInfo')
            xml.SubElement(elpw, 'contact:pw').text = pw
        self.gen_unspec(unspec)
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))

 

    def contact_delete(self, contact_id=None):
        self.gen_head()
        eldelete = xml.SubElement(self.elcommand, 'delete')
        elcontactdelete = xml.SubElement(eldelete, 'contact:delete', {'xmlns:contact':"urn:ietf:params:xml:ns:contact-1.0"})
        xml.SubElement(elcontactdelete, 'contact:id').text = contact_id
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))



 

    def domain_create(self, domain_name=None, term=1, ns=None, registrant=None, admin=None, tech=None, billing=None, pw=None, unspec=None, nvtup=None, ds_record=None):
 
        self.gen_head()
        elcreate = xml.SubElement(self.elcommand, 'create')
        eldomaincreate = xml.SubElement(elcreate, 'domain:create', {'xmlns:domain':"urn:ietf:params:xml:ns:domain-1.0"})
        xml.SubElement(eldomaincreate, 'domain:name').text = domain_name
        xml.SubElement(eldomaincreate, 'domain:period', {'unit':"y"}).text = str(term)
        if ns:
            ns = self.gen_list(ns)
            elns = xml.SubElement(eldomaincreate, 'domain:ns')
            for nserver in ns:
                xml.SubElement(elns, 'domain:hostObj').text = nserver
        xml.SubElement(eldomaincreate, 'domain:registrant').text = registrant
        if admin:
            admin = self.gen_list(admin)
            for e in admin:
                xml.SubElement(eldomaincreate, 'domain:contact', {'type':"admin"}).text = e
        if tech:
            tech = self.gen_list(tech)
            for e in tech:
                xml.SubElement(eldomaincreate, 'domain:contact', {'type':"tech"}).text = e
        if billing:
            billing = self.gen_list(billing)
            for e in billing:
                xml.SubElement(eldomaincreate, 'domain:contact', {'type':"billing"}).text = e
        elpw = xml.SubElement(eldomaincreate, 'domain:authInfo')
        xml.SubElement(elpw, 'domain:pw').text = pw
        self.gen_unspec(unspec, nvtup, ds_record=ds_record)
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))




    def domain_check(self, domain_name=None):
        self.gen_head()
        elcheck = xml.SubElement(self.elcommand, 'check')
        eldomcheck = xml.SubElement(elcheck, 'domain:check', {'xmlns:domain':"urn:ietf:params:xml:ns:domain-1.0"})
        domain_name = self.gen_list(domain_name)
        for ea in domain_name:
            xml.SubElement(eldomcheck, 'domain:name').text = ea
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))



    def domain_info(self, domain_name=None):
        self.gen_head()
        elinfo = xml.SubElement(self.elcommand, 'info')
        eldomaininfo = xml.SubElement(elinfo, 'domain:info', {'xmlns:domain':"urn:ietf:params:xml:ns:domain-1.0"})
        xml.SubElement(eldomaininfo, 'domain:name').text = domain_name
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))



    def domain_transfer(self, domain_name=None, op=None, pw=None, term=1, unspec=None):
        self.gen_head()
        eltrans = xml.SubElement(self.elcommand, 'transfer', {'op':op})
        eldomtrans = xml.SubElement(eltrans, 'domain:transfer', {'xmlns:domain':"urn:ietf:params:xml:ns:domain-1.0"})
        xml.SubElement(eldomtrans, 'domain:name').text = domain_name
        if op.lower() == 'request':
            xml.SubElement(eldomtrans, 'domain:period').text = str(term)
            elpw = xml.SubElement(eldomtrans, 'domain:authInfo')
            pw = xml.SubElement(elpw, 'domain:pw').text = pw
        self.gen_unspec(unspec)
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))




    def domain_update(self, domain_name=None, add_ns=None, remove_ns=None, registrant=None, add_admin=None, remove_admin=None, add_tech=None, remove_tech=None, add_billing=None, remove_billing=None, pw=None, add_status=None, remove_status=None, unspec=None, nvtup=None, add_ds_record=None, remove_ds_record=None):

        self.gen_head()
        elupdate = xml.SubElement(self.elcommand, 'update')
        eldomainupdate = xml.SubElement(elupdate, 'domain:update', {'xmlns:domain':"urn:ietf:params:xml:ns:domain-1.0"})
        xml.SubElement(eldomainupdate, 'domain:name').text = domain_name
        if any((add_ns, add_admin, add_tech, add_billing, add_status)):
            eladd = xml.SubElement(eldomainupdate, 'domain:add')
            if add_ns:
                elnsadd = xml.SubElement(eladd, 'domain:ns')
                add_ns = self.gen_list(add_ns)
                for ns in add_ns:
                    xml.SubElement(elnsadd, 'domain:hostObj').text = ns
            if add_admin:
                add_admin = self.gen_list(add_admin)
                for ea in add_admin:
                    xml.SubElement(eladd, 'domain:contact', {'type':"admin"}).text = ea
            if add_billing:
                add_billing = self.gen_list(add_billing)
                for ea in add_billing:
                    xml.SubElement(eladd, 'domain:contact', {'type':'billing'}).text = ea
            if add_tech:
                add_tech = self.gen_list(add_tech)
                for ea in add_tech:            
                    xml.SubElement(eladd, 'domain:contact', {'type':'tech'}).text = ea
            if add_status:
                add_status = self.gen_list(add_status)
                for ea in add_status:
                    xml.SubElement(eladd, 'domain:status', {'s':ea})
        if any((remove_ns, remove_admin, remove_tech, remove_billing, remove_status)):
            elrem = xml.SubElement(eldomainupdate, 'domain:rem')
            if remove_ns:
                elnsrem = xml.SubElement(elrem, 'domain:ns')
                remove_ns = self.gen_list(remove_ns)
                for ns in remove_ns:
                    xml.SubElement(elnsrem, 'domain:hostObj').text = ns
            if remove_admin:
                remove_admin = self.gen_list(remove_admin)
                for ea in remove_admin:
                    xml.SubElement(eldomainupdate, 'domain:contact', {'type':"admin"}).text = ea
            if remove_billing:
                remove_billing = self.gen_list(remove_billing)
                for ea in remove_billing:
                    xml.SubElement(eldomainupdate, 'domain:contact', {'type':"billing"}).text = ea
            if remove_tech:
                remove_tech = self.gen_list(remove_tech)
                for ea in remove_tech:
                    xml.SubElement(eldomainupdate, 'domain:contact', {'type':"tech"}).text = ea
            if remove_status:
                remove_status = self.gen_list(remove_status)
                for ea in remove_status:
                    xml.SubElement(elrem, 'domain:status', {'s':ea})
        if registrant or pw:
            elchg = xml.SubElement(eldomainupdate, 'domain:chg')
            if registrant:
                xml.SubElement(elchg, 'domain:registrant').text = registrant
            if pw:
                elauth = xml.SubElement(elchg, 'domain:authInfo')
                xml.SubElement(elauth, 'domain:pw').text = pw
        self.gen_unspec(unspec, nvtup, add_ds_record=add_ds_record, remove_ds_record=remove_ds_record)
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))



    def domain_delete(self, domain_name=None):
        self.gen_head()
        eldelete = xml.SubElement(self.elcommand, 'delete')
        eldomdelete = xml.SubElement(eldelete, 'domain:delete', {'xmlns:domain':"urn:ietf:params:xml:ns:domain-1.0"})
        xml.SubElement(eldomdelete, 'domain:name').text = domain_name
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))



    def domain_renew(self, domain_name=None, exp_date=None, term=None, unspec=None):
        self.gen_head()
        elrenew = xml.SubElement(self.elcommand, 'renew')
        eldomrenew = xml.SubElement(elrenew, 'domain:renew', {'xmlns:domain':"urn:ietf:params:xml:ns:domain-1.0"})
        xml.SubElement(eldomrenew, 'domain:name').text = domain_name
        xml.SubElement(eldomrenew, 'domain:curExpDate').text = exp_date
        if term:
            xml.SubElement(eldomrenew, 'domain:period', {'unit':"y"}).text=str(term)
        self.gen_unspec(unspec)
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))



    def domain_restore(self, domain_name=None, exp_date=None, restore_reason_code=None, restore_comment=None):
        return self.domain_renew(domain_name, exp_date, unspec=['RestoreReasonCode=%s' % str(restore_reason_code), 'RestoreComment=%s' % restore_comment, 'TrueData=Y', 'ValidUse=Y'])




    def host_create(self, hostname=None, ip=None, unspec=None):
        self.gen_head()
        elcreate = xml.SubElement(self.elcommand, 'create')
        elhostcreate = xml.SubElement(elcreate, 'host:create', {'xmlns:host':"urn:ietf:params:xml:ns:host-1.0"})
        xml.SubElement(elhostcreate, 'host:name').text = hostname
        if ip:
           ip = self.gen_list(ip)
           for i in ip:
               if i.count(':') > 1:
                   ver = 'v6'
               else:
                   ver = 'v4'
               xml.SubElement(elhostcreate, 'host:addr', {'ip':ver}).text = i
        self.gen_unspec(unspec)
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))



    def host_update(self, hostname=None, add_ip=None, remove_ip=None, new_hostname=None, add_status=None, remove_status=None, unspec=None):
        self.gen_head()
        elupdate = xml.SubElement(self.elcommand, 'update')
        elhostupdate = xml.SubElement(elupdate, 'host:update', {'xmlns:host':"urn:ietf:params:xml:ns:host-1.0"})
        xml.SubElement(elhostupdate, 'host:name').text = hostname
        if add_ip or add_status:
            eladd = xml.SubElement(elhostupdate,'host:add')
            if add_ip:
                add_ip = self.gen_list(add_ip)
                for ip in add_ip:
                    if ip.count(':') > 1:
                        ver = 'v6'
                    else:
                        ver = 'v4'
                    xml.SubElement(eladd, 'host:addr', {'ip':ver}).text = ip
            if add_status:
                add_status = self.gen_list(add_status)
                for stat in add_status:
                    xml.SubElement(eladd, 'host:status', {'s':stat})

        if remove_ip or remove_status:
            elrem = xml.SubElement(elhostupdate,'host:rem')
            if remove_ip:
                remove_ip = self.gen_list(remove_ip)
                for ip in remove_ip:
                    if ip.count(':') > 1:
                        ver = 'v6'
                    else:
                        ver = 'v4'
                    xml.SubElement(elrem, 'host:addr', {'ip':ver}).text = ip
            if remove_status:
                remove_status = self.gen_list(remove_status)
                for stat in remove_status:
                    xml.SubElement(elrem, 'host:status', {'s':stat})
        if new_hostname:
            elchg = xml.SubElement(elhostupdate,'host:chg')
            xml.SubElement(elchg,'host:name').text = new_hostname
        self.gen_unspec(unspec)
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))



    def host_delete(self, hostname=None):
        self.gen_head()
        eldelete = xml.SubElement(self.elcommand, 'delete')
        elhostdelete = xml.SubElement(eldelete, 'host:delete', {'xmlns:host':"urn:ietf:params:xml:ns:host-1.0"})
        xml.SubElement(elhostdelete, 'host:name').text = hostname
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))



    def host_check(self, hostname=None):
        self.gen_head()
        elcheck = xml.SubElement(self.elcommand, 'check')
        elhostcheck = xml.SubElement(elcheck, 'host:check', {'xmlns:host':"urn:ietf:params:xml:ns:host-1.0"})
        hostname = self.gen_list(hostname)
        for host in hostname:
            xml.SubElement(elhostcheck, 'host:name').text = host
        self.gen_tail()
        return(self.top + xml.tostring(self.epp,'unicode'))



    def host_info(self, hostname=None):
       self.gen_head()
       elinfo = xml.SubElement(self.elcommand, 'info')
       elhostinfo = xml.SubElement(elinfo, 'host:info', {'xmlns:host':"urn:ietf:params:xml:ns:host-1.0"})
       xml.SubElement(elhostinfo, 'host:name').text = hostname
       self.gen_tail()
       return(self.top + xml.tostring(self.epp,'unicode'))


    def pretty(self, xmlin):
        curlist = []
        slist = []
        count=0
        xmlin = xmlin.replace('><', '>\n<')
        xmlin = xmlin.split('\n')
        for lin in xmlin:
            if '</' not in lin and ' />' not in lin:
                slist.append((count) * ' ' + lin)
                count += 1
            elif (lin.count('<') > 1 and lin.count('>') > 1) or ' />' in lin:
                slist.append((count) * ' ' + lin)
            else:
                count -= 1
                slist.append((count) * ' ' + lin)
        return '\n'.join(slist)

    def send_file(self, file_name):
        try:
            in_file = open(file_name, 'r')
            in_file_contents = in_file.read()
            in_file.close()
        except:
            print("Error opening file %s" % file_name)
            sys.exit(1)
        return in_file_contents
