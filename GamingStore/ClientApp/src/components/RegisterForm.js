import React, { Component } from "react";

export class RegisterForm extends Component {
    static displayName = RegisterForm.name;

    constructor(props) {
        super(props);

        this.state = {
            username: "", 
            password: "", 
            confirmPassword: ""
        };
        
        this.onUsernameChange = this.onUsernameChange.bind(this);
        this.onPasswordChange = this.onPasswordChange.bind(this);
        this.onConfirmPasswordChange = this.onConfirmPasswordChange.bind(this);
        this.registerUser = this.registerUser.bind(this);
    }

    onUsernameChange(e) {
        this.setState({username: e.target.value});
    }

    onPasswordChange(e) {
        this.setState({password: e.target.value});
    }

    onConfirmPasswordChange(e) {
        this.setState({confirmPassword: e.target.value});
    }

    async registerUser(e) {
        e.preventDefault();

        fetch("../api/users/register", {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
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
                window.location.href = "/login";
            })
            .catch(error => alert(error.message));
    }

    render() {
        return <form onSubmit={this.registerUser}>
            <div className="form-group mb-3">
                <label htmlFor="username" className="control-label">Login:</label>
                <input id="username" className="form-control" type="text" value={this.state.username} onChange={this.onUsernameChange} />
            </div>
            <div className="form-group mb-3">
                <label htmlFor="password" className="control-label">Password:</label>
                <input id="password" className="form-control" type="password" value={this.state.password} onChange={this.onPasswordChange} />
            </div>
            <div className="form-group mb-3">
                <label htmlFor="confirm-password" className="control-label">Confirm password:</label>
                <input id="confirm-password" className="form-control" type="password" value={this.state.confirmPassword} onChange={this.onConfirmPasswordChange} />
            </div>

            <input type="submit" className="btn btn-primary" value="Sign Up" />
        </form>;
    }
}