import React, { Component } from 'react';

export class LoginForm extends Component {
    static displayName = LoginForm.name;

    constructor(props) {
        super(props);

        this.state = {
            username: '', 
            password: ''
        };
        
        this.loginUser = this.loginUser.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
    }

    async loginUser(event) {
        event.preventDefault();

        await fetch('../api/users/login', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(this.state)
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                }

                return response.text().then(error => { throw new Error(error) });
            })
            .then(data => {
                sessionStorage.setItem('token', data.token);
                sessionStorage.setItem('role', data.role);
                sessionStorage.setItem('username', data.username);

                window.location.href = '/';
            })
            .catch(error => alert(error.message));
    }

    handleInputChange = event => {
        const name = event.target.name;
        const value = event.target.value;

        this.setState({
            [name]: value
        });
    }

    render() {
        return <form onSubmit={this.loginUser}>
            <div className="form-group mb-3">
                <label htmlFor="username" className="control-label">Login:</label>
                <input id="username" className="form-control" type="text" name="username" value={this.state.username} onChange={this.handleInputChange} />
            </div>
            <div className="form-group mb-3">
                <label htmlFor="password" className="control-label">Password:</label>
                <input id="password" className="form-control" type="password" name="password" value={this.state.password} onChange={this.handleInputChange} />
            </div>

            <input className="btn btn-primary" type="submit" value="Login" />
        </form>;
    }
}