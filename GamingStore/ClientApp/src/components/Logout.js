import { Component } from 'react';

export class Logout extends Component {
    static displayName = Logout.name;

    componentDidMount() {
        sessionStorage.clear();
        window.location.href = '/login';
    }

    render() {
        return null;
    }
}