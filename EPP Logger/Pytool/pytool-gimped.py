#!/usr/bin/python3
import time, sys, select, threading, os
import epp.epp10 as epp10
import epp.epp04 as epp04

tlds = ('biz', 'co', 'us', 'tel', 'travel', 'tw', 'cn', 'cnidn', 'gsma', 'iq')
types = ('ote', 'prod', 'local', 'remote')
commands = ('hello', 'domain_restore', 'poll', 'contact_create', 'contact_delete', 'contact_update', 'contact_info', 'contact_check', 'contact_transfer', 'domain_create', 'domain_delete', 'domain_update', 'domain_info', 'domain_check', 'domain_transfer', 'domain_renew', 'host_create', 'host_delete', 'host_update', 'host_info', 'host_check', 'send_file', 'login')

def parseargs(args):
    argdic={}
    (tld, type, commandtorun, extra) = (None, None, None, None)
    if len(args) < 3:
        print("To use this program, you must specify arguments.  For example <program>.py biz ote domain_info domain_name=neustar.biz .  It doesn't matter where any of the arguments go...so use any order you want to.  You can also specify how many times to run it.  Such as: 100 domain_info domain_name=neustar.biz biz prod.\nYou can also feed lists instead of domains.  The program detects whether or not the domain, contact, or host entered is a file or not.  If it's a file, it will iterate through the file.  For example:  <program> domain_info domain_name=listofdomain.txt biz ote.  This can be combined with how many times to run, but I don't know why you'd do that.\nCommands are: \n%s" % '\n'.join(commands))
        sys.exit(1)
    del args[0]
    for arg in args:
        argl = arg.lower()
        if argl in tlds:
            tld = argl
        elif argl in types:
            type = argl
        elif argl in commands:
            commandtorun = argl
        elif '=' in arg:
            arg = arg.split('=')
            if len(arg) > 2:
                arg[1] = '='.join(arg[1:])
            argdic[arg[0]] = arg[1]
    if tld == None or commandtorun == None:
        print('Must specify TLD and ENVIRONMENT.  Can be anywhere, but should be seperate args.  Run command with no args to see example')
        print(tld, type, commandtorun, args, commands)
        sys.exit(1)  
            
    return(tld, type, commandtorun, argdic)


def print_xml(tld, commandtorun, argdic):
    if tld in ('biz', 'us', 'co', 'tel', 'travel', 'gsma', 'iq'):
        myepp = epp10.EPP10()
    elif tld in ('cn', 'cnidn','tw'):
        myepp = epp04.EPP04()
    xz = getattr(myepp, commandtorun)
    print(myepp.pretty(xz(**argdic)))

def main(): 
    (tld, type, commandtorun, argdic) = parseargs(sys.argv)
    print_xml(tld, commandtorun, argdic)

main()

