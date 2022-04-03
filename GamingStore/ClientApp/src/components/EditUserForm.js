import React, { Component } from 'react';

export class EditUserForm extends Component {
    static displayName = EditUserForm.name;

    constructor(props) {
        super(props);

        this.state = {
            username: '',
            role: 1,
            token: sessionStorage.getItem('token')
        };

        this.handleInputChange = this.handleInputChange.bind(this);
        this.updateUser = this.updateUser.bind(this);
    }

    handleInputChange(event) {
        const name = event.target.name;
        const value = event.target.value;

        this.setState({
            [name]: value
        });
    }

    async updateUser(event) {
        event.preventDefault();

        await fetch(`../api/users/${window.location.pathname.split('/')[3]}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.state.token}`
            },
            body: JSON.stringify(this.state)
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(error => { throw new Error(error) });
                }
            })
            .then(() => window.location.href = '/users')
            .catch(error => alert(error.message));
    }

    render() {
        return <form onSubmit={this.updateUser}>
            <div className="form-group mb-3">
                    <label htmlFor="new-usernmae" className="control-label">Login:</label>
                    <input id="new-usernmae" className="form-control" type="text" name="username" value={this.state.username} onChange={this.handleInputChange} />
                </div>
                <div className="form-group mb-3">
                    <label htmlFor="new-role" className="control-label">Role:</label>
                    <select id="new-role" className="form-control" name="role" value={this.state.role} onChange={this.handleInputChange}>
                        <option value="0">Admin</option>
                        <option value="1">User</option>
                    </select>
                </div>

                <input className="btn btn-primary" type="submit" value="Update" />
        </form>;
    }
}