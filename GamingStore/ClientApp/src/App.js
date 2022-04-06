import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Users } from './components/Users';
import { EditUserForm } from './components/EditUserForm';
import { Companies } from './components/Companies';
import { AddCompanyForm } from './components/AddComanyForm';
import { EditCompanyForm } from './components/EditCompanyForm';
import { Devices } from './components/Devices';
import { AddDeviceForm } from './components/AddDeviceForm';
import { EditDeviceForm } from './components/EditDeviceForm';
import { LoginForm } from './components/LoginForm';
import { RegisterForm } from './components/RegisterForm';
import { Logout } from './components/Logout';

import './custom.css'

export default class App extends Component {
    static displayName = App.name;

    render () {
        return (
            <Layout>
                <Route exact path='/' component={Home} />
                <Route path='/users' component={Users} />
                <Route path='/users/edit/*' component={EditUserForm} />
                <Route path='/companies' component={Companies} />
                <Route path='/companies/add' component={AddCompanyForm} />
                <Route path='/companies/edit/*' component={EditCompanyForm} />
                <Route path='/devices' component={Devices} />
                <Route path='/devices/add' component={AddDeviceForm} />
                <Route path='/devices/edit/*' component={EditDeviceForm} />
                <Route path='/login' component={LoginForm} />
                <Route path='/register' component={RegisterForm} />
                <Route path='/logout' component={Logout} />
            </Layout>
        );
    }
}
