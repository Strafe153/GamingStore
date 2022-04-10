import React, { Component } from 'react';

export class RegisterForm extends Component {
    static displayName = RegisterForm.name;

    constructor(props) {
        super(props);

        this.state = {
            username: '', 
            password: '', 
            confirmPassword: ''
        };
        
        this.handleInputChange = this.handleInputChange.bind(this);
        this.registerUser = this.registerUser.bind(this);
    }

    async registerUser(event) {
        event.preventDefault();

        await fetch('../api/users/register', {
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
            .then(() => {
                window.location.href = '/login';
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
        return <form onSubmit={this.registerUser}>
            <div className="form-group mb-3">
                <label htmlFor="username" className="control-label">Login:</label>
                <input id="username" className="form-control" type="text" name="username" value={this.state.username} onChange={this.handleInputChange} />
            </div>
            <div className="form-group mb-3">
                <label htmlFor="password" className="control-label">Password:</label>
                <input id="password" className="form-control" type="password" name="password" value={this.state.password} onChange={this.handleInputChange} />
            </div>
            <div className="form-group mb-3">
                <label htmlFor="confirm-password" className="control-label">Confirm password:</label>
                <input id="confirm-password" className="form-control" type="password" name="confirmPassword" value={this.state.confirmPassword} onChange={this.handleInputChange} />
            </div>

            <input type="submit" className="btn btn-primary" value="Sign Up" />
        </form>;
    }
}