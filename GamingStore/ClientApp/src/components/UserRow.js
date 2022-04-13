import React, { Component} from 'react';

export class UserRow extends Component {
    static displayName = UserRow.name;

    constructor(props) {
        super(props);

        this.state = {
            token: sessionStorage.getItem('token')
        };
    }

    async deleteUser(id) {
        await fetch(`../api/users/${id}`, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.state.token}`
            }
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(error => { throw new Error(error) });
                }
            })
            .catch(error => alert(error.message));
    }

    updateUser(id) {
        window.location.href = `/users/edit/${id}`;
    }

    render() {
        const userId = this.props.id;

        return <tr>
            <td className="text-center">
                <img src={ `data:image/png;base64,${this.props.profilePicture}` } alt="user-profile-pic" width="75" />
            </td>
            <td>{ userId }</td>
            <td>{ this.props.username }</td>
            <td>{ this.props.role }</td>
            <td>
            {(this.props.calledFromAdmin || (sessionStorage.getItem('username') === this.props.username)) &&
                <div className="text-center d-flex justify-content-around">
                    <button onClick={() => this.updateUser(userId)} className="btn btn-sm btn-info text-white">Edit</button>
                    <button onClick={
                        async () => { 
                            await this.deleteUser(userId); 
                            await this.props.getUsers(); 
                        }
                    } className="btn btn-sm btn-danger">Delete</button>
                </div>
            }
            </td>
        </tr>;
    }
}