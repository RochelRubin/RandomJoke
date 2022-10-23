import React, { Component, useEffect } from 'react';
import { Route } from 'react-router-dom';
import Layout from './components/Layout';
import HomePage from './pages/Home';
import LogIn from './pages/Login';
import Logout from './pages/Logout';
import SignUp from './pages/Signup';
import ViewAll from './pages/ViewAll';
import { AuthContextComponent } from './AuthContext';

export default class App extends Component {
    render() {
        return (
            <AuthContextComponent>
                <Layout>
                    <Route exact path='/' component={HomePage} />
                    <Route exact path='/viewAll' component={ViewAll} />
                    <Route exact path='/signUp' component={SignUp} />
                    <Route exact path='/login' component={LogIn} />
                    <Route exact path='/logout' component={Logout} />

                </Layout>
            </AuthContextComponent>
        );
    }
}